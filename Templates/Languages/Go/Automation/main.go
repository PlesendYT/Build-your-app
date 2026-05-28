package main

import (
	"flag"
	"fmt"
	"log"
	"os"
	"os/exec"
	"os/signal"
	"path/filepath"
	"strings"
	"syscall"
	"time"

	"github.com/fsnotify/fsnotify"
	"github.com/robfig/cron/v3"
)

func runCommand(name string, args ...string) {
	cmd := exec.Command(name, args...)
	cmd.Stdout = os.Stdout
	cmd.Stderr = os.Stderr
	if err := cmd.Run(); err != nil {
		log.Printf("Command failed: %v", err)
	}
}

func watchDir(dir string) {
	watcher, err := fsnotify.NewWatcher()
	if err != nil {
		log.Fatal(err)
	}
	defer watcher.Close()

	done := make(chan bool)
	go func() {
		for {
			select {
			case event, ok := <-watcher.Events:
				if !ok {
					return
				}
				log.Printf("Event: %s %s", event.Op, event.Name)
				if event.Op&(fsnotify.Create|fsnotify.Write) != 0 {
					if strings.HasSuffix(event.Name, ".go") {
						runCommand("go", "build", "./...")
					}
				}
			case err, ok := <-watcher.Errors:
				if !ok {
					return
				}
				log.Printf("Error: %v", err)
			}
		}
	}()

	filepath.Walk(dir, func(path string, info os.FileInfo, err error) error {
		if err != nil {
			return err
		}
		if info.IsDir() {
			return watcher.Add(path)
		}
		return nil
	})

	log.Printf("Überwache: %s (Drücke Ctrl+C zum Beenden)", dir)
	<-done
}

func runScheduler(spec string, cmdName string, args []string) {
	c := cron.New()
	_, err := c.AddFunc(spec, func() {
		log.Printf("Ausführung: %s %s", cmdName, strings.Join(args, " "))
		runCommand(cmdName, args...)
	})
	if err != nil {
		log.Fatal(err)
	}
	c.Start()

	log.Printf("Scheduler läuft: %s -> %s %s", spec, cmdName, strings.Join(args, " "))
	log.Println("Drücke Ctrl+C zum Beenden")

	sc := make(chan os.Signal, 1)
	signal.Notify(sc, syscall.SIGINT, syscall.SIGTERM)
	<-sc
	c.Stop()
}

func main() {
	var mode string
	var dir string
	var spec string
	flag.StringVar(&mode, "mode", "watch", "watch or schedule")
	flag.StringVar(&dir, "dir", ".", "directory to watch")
	flag.StringVar(&spec, "spec", "*/5 * * * *", "cron spec")
	flag.Parse()

	switch mode {
	case "watch":
		watchDir(dir)
	case "schedule":
		if flag.NArg() < 1 {
			fmt.Println("Usage: automation -mode schedule -spec \"*/5 * * * *\" <command> [args...]")
			return
		}
		runScheduler(spec, flag.Arg(0), flag.Args()[1:])
	default:
		fmt.Printf("Unknown mode: %s (use watch or schedule)\n", mode)
	}
}
