// Package main implements a simple TCP chat server and client
// supporting echo, broadcast, and graceful shutdown.
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

// clients stores connected TCP connections keyed by remote address.
var clients sync.Map

// runServer starts a TCP listener on the given port, accepting connections
// and handling each in a goroutine. Supports graceful shutdown on signal.
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

// handleConnection manages a single client session: reads messages,
// echoes them back, supports /broadcast and /quit commands.
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

// runClient connects to a TCP server, reads server messages in a
// background goroutine, and sends user input from stdin.
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

// main parses flags and starts either the server or client mode.
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
