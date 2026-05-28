package main

import (
	"encoding/json"
	"log"
	"net/http"
	"sync"

	"github.com/gorilla/websocket"
)

var upgrader = websocket.Upgrader{
	CheckOrigin: func(r *http.Request) bool { return true },
}

type Message struct {
	Type    string `json:"type"`
	Content string `json:"content"`
	Sender  string `json:"sender"`
}

type Hub struct {
	mu      sync.RWMutex
	clients map[*websocket.Conn]string
}

var hub = &Hub{clients: make(map[*websocket.Conn]string)}

func (h *Hub) broadcast(msg Message) {
	h.mu.RLock()
	defer h.mu.RUnlock()
	data, _ := json.Marshal(msg)
	for conn := range h.clients {
		conn.WriteMessage(websocket.TextMessage, data)
	}
}

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
