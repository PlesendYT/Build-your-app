// Package main implements a WebSocket-based chat server that
// broadcasts messages to all connected clients.
package main

import (
	"encoding/json"
	"log"
	"net/http"
	"sync"

	"github.com/gorilla/websocket"
)

// upgrader upgrades HTTP connections to WebSocket, accepting all origins.
var upgrader = websocket.Upgrader{
	CheckOrigin: func(r *http.Request) bool { return true },
}

// Message represents a chat message exchanged between clients.
type Message struct {
	Type    string `json:"type"`
	Content string `json:"content"`
	Sender  string `json:"sender"`
}

// Hub manages connected WebSocket clients and their associated names.
type Hub struct {
	mu      sync.RWMutex
	clients map[*websocket.Conn]string
}

// hub is the global Hub instance managing all active connections.
var hub = &Hub{clients: make(map[*websocket.Conn]string)}

// broadcast sends a JSON-encoded message to every connected client.
func (h *Hub) broadcast(msg Message) {
	h.mu.RLock()
	defer h.mu.RUnlock()
	data, _ := json.Marshal(msg)
	for conn := range h.clients {
		conn.WriteMessage(websocket.TextMessage, data)
	}
}

// handleWS upgrades an HTTP connection to WebSocket, registers the client
// in the hub, broadcasts join/leave messages, and relays incoming messages.
func handleWS(w http.ResponseWriter, r *http.Request) {
	conn, err := upgrader.Upgrade(w, r, nil)
	if err != nil {
		log.Printf("Upgrade failed: %v", err)
		return
	}
	defer conn.Close()

	name := r.URL.Query().Get("name")
	if name == "" {
		name = "Anonymous"
	}

	hub.mu.Lock()
	hub.clients[conn] = name
	hub.mu.Unlock()

	hub.broadcast(Message{
		Type:    "join",
		Content: name + " ist beigetreten",
		Sender:  "system",
	})

	for {
		_, data, err := conn.ReadMessage()
		if err != nil {
			break
		}
		var msg Message
		if err := json.Unmarshal(data, &msg); err != nil {
			continue
		}
		msg.Sender = name
		hub.broadcast(msg)
	}

	hub.mu.Lock()
	delete(hub.clients, conn)
	hub.mu.Unlock()

	hub.broadcast(Message{
		Type:    "leave",
		Content: name + " hat verlassen",
		Sender:  "system",
	})
}

// main sets up the HTTP routes and starts the WebSocket server on :8080.
func main() {
	http.HandleFunc("/ws", handleWS)
	http.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Content-Type", "text/html")
		w.Write([]byte(`<h1>WebSocket Server</h1>
<p>Verbinde via: <code>ws://localhost:8080/ws?name=DeinName</code></p>
<script>
const ws = new WebSocket('ws://' + location.host + '/ws?name=Test');
ws.onmessage = e => console.log(JSON.parse(e.data));
</script>`))
	})

	log.Println("WebSocket-Server läuft auf :8080")
	log.Fatal(http.ListenAndServe(":8080", nil))
}
