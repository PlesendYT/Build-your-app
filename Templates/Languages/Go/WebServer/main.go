// Package main implements a simple REST API server using only the standard
// library, with middleware for logging and CORS.
package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"time"
)

// Todo represents a to-do item stored in the in-memory slice.
type Todo struct {
	ID        int       `json:"id"`
	Title     string    `json:"title"`
	Completed bool      `json:"completed"`
	CreatedAt time.Time `json:"created_at"`
}

// todos is the in-memory store of Todo items, pre-populated with one entry.
var todos = []Todo{
	{ID: 1, Title: "Go lernen", Completed: false, CreatedAt: time.Now()},
}

// nextID is the auto-incrementing ID counter for new todos.
var nextID = 2

// loggingMiddleware logs every request method, path, and client IP
// along with the total handling duration.
func loggingMiddleware(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		start := time.Now()
		log.Printf("%s %s %s", r.Method, r.URL.Path, r.RemoteAddr)
		next.ServeHTTP(w, r)
		log.Printf("Dauer: %v", time.Since(start))
	})
}

// corsMiddleware adds permissive CORS headers and handles OPTIONS preflight.
func corsMiddleware(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Access-Control-Allow-Origin", "*")
		w.Header().Set("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
		w.Header().Set("Access-Control-Allow-Headers", "Content-Type")
		if r.Method == "OPTIONS" {
			w.WriteHeader(http.StatusOK)
			return
		}
		next.ServeHTTP(w, r)
	})
}

// handleTodos serves GET (list all) and POST (create new) on /api/todos.
func handleTodos(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")

	switch r.Method {
	case http.MethodGet:
		json.NewEncoder(w).Encode(todos)

	case http.MethodPost:
		var todo Todo
		if err := json.NewDecoder(r.Body).Decode(&todo); err != nil {
			http.Error(w, err.Error(), http.StatusBadRequest)
			return
		}
		todo.ID = nextID
		nextID++
		todo.CreatedAt = time.Now()
		todos = append(todos, todo)
		w.WriteHeader(http.StatusCreated)
		json.NewEncoder(w).Encode(todo)

	default:
		http.Error(w, "Method not allowed", http.StatusMethodNotAllowed)
	}
}

// handleIndex serves the root HTML page with a link to the API.
func handleIndex(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "text/html")
	fmt.Fprint(w, `<h1>Go WebServer</h1><p>API: <a href="/api/todos">/api/todos</a></p>`)
}

// main configures routes, wraps them with middleware, and starts the
// HTTP server on :8080 with read/write timeouts.
func main() {
	mux := http.NewServeMux()
	mux.HandleFunc("/", handleIndex)
	mux.HandleFunc("/api/todos", handleTodos)

	handler := loggingMiddleware(corsMiddleware(mux))

	server := &http.Server{
		Addr:         ":8080",
		Handler:      handler,
		ReadTimeout:  10 * time.Second,
		WriteTimeout: 10 * time.Second,
	}

	log.Println("Server läuft auf :8080")
	if err := server.ListenAndServe(); err != nil {
		log.Fatal(err)
	}
}
