// Simple Go HTTP server that serves as a Kubernetes deployment demo
// Responds to /health (liveness) and / (greeting) endpoints

package main

import (
	"fmt"
	"log"
	"net/http"
	"os"
)

// Read configuration from environment variables (injected via ConfigMap)
var (
	port    = getEnv("APP_PORT", "8080")
	appEnv  = getEnv("APP_ENV", "development")
	logLevel = getEnv("LOG_LEVEL", "debug")
)

// getEnv returns the value of an environment variable or a fallback default
func getEnv(key, fallback string) string {
	if val, ok := os.LookupEnv(key); ok {
		return val
	}
	return fallback
}

// healthHandler is used by liveness and readiness probes
func healthHandler(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusOK)
	fmt.Fprint(w, `{"status":"healthy"}`)
}

// rootHandler returns a simple greeting with environment info
func rootHandler(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusOK)
	fmt.Fprintf(w, `{"message":"Hello from Go!","env":"%s"}`, appEnv)
}

func main() {
	// Register HTTP handlers
	http.HandleFunc("/health", healthHandler)
	http.HandleFunc("/", rootHandler)

	log.Printf("Starting server on port %s (env: %s, log: %s)", port, appEnv, logLevel)
	if err := http.ListenAndServe(":"+port, nil); err != nil {
		log.Fatalf("Server failed: %v", err)
	}
}
