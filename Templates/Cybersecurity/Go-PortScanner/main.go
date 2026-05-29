// main.go — Concurrent TCP port scanner with configurable timeout
// Scans a target host for open ports within a given range using goroutines.

package main

import (
	"flag"
	"fmt"
	"net"
	"os"
	"sort"
	"strconv"
	"strings"
	"sync"
	"time"
)

// ScanResult holds the result of probing a single port
type ScanResult struct {
	Port  int
	State string // "open" or "closed"
}

// scanPort attempts a TCP connection to the given port and returns the result
func scanPort(host string, port int, timeout time.Duration) ScanResult {
	addr := net.JoinHostPort(host, strconv.Itoa(port))
	conn, err := net.DialTimeout("tcp", addr, timeout)
	if err != nil {
		return ScanResult{Port: port, State: "closed"}
	}
	conn.Close()
	return ScanResult{Port: port, State: "open"}
}

// scanRange scans ports from start to end concurrently using a worker pool
func scanRange(host string, start, end int, timeout time.Duration, workers int) []ScanResult {
	var wg sync.WaitGroup
	results := make([]ScanResult, 0, end-start+1)
	resultCh := make(chan ScanResult, end-start+1)
	portCh := make(chan int, workers)

	// Launch worker goroutines
	for i := 0; i < workers; i++ {
		wg.Add(1)
		go func() {
			defer wg.Done()
			for port := range portCh {
				resultCh <- scanPort(host, port, timeout)
			}
		}()
	}

	// Feed port numbers to workers
	go func() {
		for port := start; port <= end; port++ {
			portCh <- port
		}
		close(portCh)
	}()

	// Wait for all workers to finish, then close the result channel
	go func() {
		wg.Wait()
		close(resultCh)
	}()

	// Collect results
	for r := range resultCh {
		results = append(results, r)
	}

	// Sort by port number for clean output
	sort.Slice(results, func(i, j int) bool {
		return results[i].Port < results[j].Port
	})
	return results
}

func main() {
	// Command-line flags with defaults
	host := flag.String("host", "127.0.0.1", "Target hostname or IP address")
	ports := flag.String("ports", "1-1024", "Port range (e.g. 80, 80-443, or 22,80,443)")
	timeout := flag.Duration("timeout", 2*time.Second, "Connection timeout per port")
	workers := flag.Int("workers", 50, "Number of concurrent workers")
	flag.Parse()

	// Parse the port specification
	var portList []int
	for _, part := range strings.Split(*ports, ",") {
		part = strings.TrimSpace(part)
		if strings.Contains(part, "-") {
			bounds := strings.SplitN(part, "-", 2)
			start, _ := strconv.Atoi(strings.TrimSpace(bounds[0]))
			end, _ := strconv.Atoi(strings.TrimSpace(bounds[1]))
			for p := start; p <= end; p++ {
				portList = append(portList, p)
			}
		} else {
			p, _ := strconv.Atoi(part)
			portList = append(portList, p)
		}
	}

	if len(portList) == 0 {
		fmt.Fprintln(os.Stderr, "No valid ports specified.")
		os.Exit(1)
	}

	fmt.Printf("Scanning %s on %d ports (workers=%d, timeout=%s)...\n",
		*host, len(portList), *workers, *timeout)

	// Perform the scan
	results := scanRange(*host, portList[0], portList[len(portList)-1], *timeout, *workers)

	// Print open ports in a formatted table
	fmt.Println("\nPORT\tSTATE\tSERVICE")
	openCount := 0
	for _, r := range results {
		if r.State == "open" {
			svc := guessService(r.Port)
			fmt.Printf("%d/tcp\t%s\t%s\n", r.Port, r.State, svc)
			openCount++
		}
	}
	fmt.Printf("\n%d ports open on %s\n", openCount, *host)
}

// guessService returns a well-known service name for a port, or "unknown"
func guessService(port int) string {
	services := map[int]string{
		21: "ftp", 22: "ssh", 23: "telnet", 25: "smtp", 53: "dns",
		80: "http", 110: "pop3", 143: "imap", 443: "https", 993: "imaps",
		3306: "mysql", 5432: "postgresql", 6379: "redis", 8080: "http-alt",
	}
	if s, ok := services[port]; ok {
		return s
	}
	return "unknown"
}
