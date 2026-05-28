package main

import (
	"bufio"
	"flag"
	"fmt"
	"log"
	"net"
	"os"
	"os/signal"
	"strings"
	"sync"
	"syscall"
	"time"
)

var clients sync.Map

func runServer(port string) {
	listener, err := net.Listen("tcp", ":"+port)
	if err != nil {
		log.Fatal(err)
	}
	defer listener.Close()
	log.Printf("TCP-Server läuft auf :%s", port)

	// Graceful shutdown
	sigCh := make(chan os.Signal, 1)
	signal.Notify(sigCh, syscall.SIGINT, syscall.SIGTERM)
	go func() {
		<-sigCh
		log.Println("Server wird heruntergefahren...")
		listener.Close()
	}()

	for {
		conn, err := listener.Accept()
		if err != nil {
			break
		}
		go handleConnection(conn)
	}
}

func handleConnection(conn net.Conn) {
	defer conn.Close()
	conn.SetDeadline(time.Now().Add(5 * time.Minute))
	clients.Store(conn.RemoteAddr().String(), conn)

	fmt.Fprintf(conn, "Willkommen beim TCP-Server!\n")
	scanner := bufio.NewScanner(conn)
	for scanner.Scan() {
		msg := strings.TrimSpace(scanner.Text())
		if msg == "/quit" {
			fmt.Fprintf(conn, "Tschüss!\n")
			break
		}
		if strings.HasPrefix(msg, "/broadcast ") {
			bcast := strings.TrimPrefix(msg, "/broadcast ")
			clients.Range(func(key, value interface{}) bool {
				if c, ok := value.(net.Conn); ok {
					fmt.Fprintf(c, "[Broadcast] %s: %s\n", conn.RemoteAddr(), bcast)
				}
				return true
			})
			continue
		}
		log.Printf("[%s] %s", conn.RemoteAddr(), msg)
		fmt.Fprintf(conn, "Echo: %s\n", msg)
	}

	clients.Delete(conn.RemoteAddr().String())
}

func runClient(addr string) {
	conn, err := net.DialTimeout("tcp", addr, 10*time.Second)
	if err != nil {
		log.Fatal(err)
	}
	defer conn.Close()

	go func() {
		scanner := bufio.NewScanner(conn)
		for scanner.Scan() {
			fmt.Println(scanner.Text())
		}
	}()

	scanner := bufio.NewScanner(os.Stdin)
	for scanner.Scan() {
		line := scanner.Text()
		fmt.Fprintf(conn, line+"\n")
		if line == "/quit" {
			break
		}
	}
}

func main() {
	var mode string
	var port string
	var addr string
	flag.StringVar(&mode, "mode", "server", "server or client")
	flag.StringVar(&port, "port", "9000", "server port")
	flag.StringVar(&addr, "addr", "localhost:9000", "server address")
	flag.Parse()

	switch mode {
	case "server":
		runServer(port)
	case "client":
		runClient(addr)
	default:
		fmt.Println("Unknown mode. Use server or client.")
	}
}
