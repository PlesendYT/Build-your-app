"use client";

import { FormEvent, useState } from "react";

// Type definition for a single chat message
interface Message {
  role: "user" | "assistant";
  content: string;
}

export default function ChatPage() {
  // State for messages array and current input value
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");
  const [loading, setLoading] = useState(false);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    if (!input.trim() || loading) return;

    // Add the user message to the chat history
    const userMsg: Message = { role: "user", content: input };
    setMessages((prev) => [...prev, userMsg]);
    setInput("");
    setLoading(true);

    try {
      // Send the conversation to the API route
      const res = await fetch("/api/chat", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ messages: [...messages, userMsg] }),
      });

      const data = await res.json();
      const assistantMsg: Message = { role: "assistant", content: data.reply };
      setMessages((prev) => [...prev, assistantMsg]);
    } catch {
      // Handle network or server errors gracefully
      setMessages((prev) => [
        ...prev,
        { role: "assistant", content: "Error: Could not reach the server." },
      ]);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div style={{ maxWidth: 700, margin: "0 auto", padding: 24 }}>
      <h1 style={{ fontSize: "1.5rem", marginBottom: 16 }}>AI Chat</h1>

      {/* Message list */}
      <div style={{ display: "flex", flexDirection: "column", gap: 12, marginBottom: 16 }}>
        {messages.map((msg, i) => (
          <div
            key={i}
            style={{
              alignSelf: msg.role === "user" ? "flex-end" : "flex-start",
              background: msg.role === "user" ? "#2b5278" : "#2a2a2a",
              padding: "8px 14px",
              borderRadius: 12,
              maxWidth: "80%",
            }}
          >
            {msg.content}
          </div>
        ))}
        {loading && <div style={{ color: "#888" }}>Thinking...</div>}
      </div>

      {/* Input form */}
      <form onSubmit={handleSubmit} style={{ display: "flex", gap: 8 }}>
        <input
          value={input}
          onChange={(e) => setInput(e.target.value)}
          placeholder="Type a message..."
          style={{ flex: 1, padding: 10, borderRadius: 8, border: "1px solid #444", background: "#1a1a1a", color: "#e0e0e0" }}
        />
        <button
          type="submit"
          disabled={loading}
          style={{ padding: "10px 20px", borderRadius: 8, border: "none", background: "#2b5278", color: "#fff", cursor: "pointer" }}
        >
          Send
        </button>
      </form>
    </div>
  );
}
