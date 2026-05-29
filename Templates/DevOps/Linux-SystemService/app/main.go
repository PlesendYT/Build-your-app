// Go HTTP server designed to run as a systemd service
// Listens on port 8080 and provides health and metrics endpoints

package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"os"
	"runtime"
	"time"
)

// Build-time variables set via ldflags during compilation
var (
	BuildVersion = "dev"
	BuildTime    = "unknown"
)

// ServerStartTime records when the server was started (used for uptime)
var ServerStartTime = time.Now()

// SystemInfo holds runtime metrics returned by the /info endpoint
type SystemInfo struct {
	Version   string `json:"version"`
	Uptime    string `json:"uptime"`
	GoVersion string `json:"go_version"`
	NumCPU    int    `json:"num_cpu"`
	HeapMB    uint64 `json:"heap_mb"`
}

// healthHandler responds with a simple status for systemd health checks
func healthHandler(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusOK)
	json.NewEncoder(w).Encode(map[string]string{
		"status": "healthy",
	})
}

// infoHandler returns build info and runtime metrics
func infoHandler(w http.ResponseWriter, r *http.Request) {
	uptime := time.Since(ServerStartTime).Round(time.Second).String()

	var m runtime.MemStats
	runtime.ReadMemStats(&m)

	info := SystemInfo{
		Version:   BuildVersion,
		Uptime:    uptime,
		GoVersion: runtime.Version(),
		NumCPU:    runtime.NumCPU(),
		HeapMB:    m.Alloc / 1024 / 1024,
	}

	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(info)
}

func main() {
	// Read the listen port from the environment (systemd override)
	port := os.Getenv("PORT")
	if port == "" {
		port = "8080"
	}

	// Register HTTP routes
	http.HandleFunc("/health", healthHandler)
	http.HandleFunc("/info", infoHandler)

	log.Printf("Starting server v%s (built %s) on :%s", BuildVersion, BuildTime, port)
	if err := http.ListenAndServe(":"+port, nil); err != nil {
		log.Fatalf("Server failed: %v", err)
	}
}
