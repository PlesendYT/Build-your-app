import { NextRequest, NextResponse } from "next/server";

// Type for a chat message in the API
interface Message {
  role: "user" | "assistant";
  content: string;
}

// POST handler for the chat API endpoint
// Receives a messages array and returns an AI-generated reply
export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    const messages: Message[] = body.messages ?? [];

    // Build a prompt from the conversation history
    // In production, replace this with a call to OpenAI, Anthropic, or an open model
    const prompt = messages
      .map((m) => `${m.role === "user" ? "User" : "Assistant"}: ${m.content}`)
      .join("\n");

    // Simulate an AI response (swap this with a real API call)
    const reply = `Echo: "${messages[messages.length - 1]?.content ?? ""}" [AI response placeholder]`;

    // Return the reply as JSON
    return NextResponse.json({ reply });
  } catch {
    return NextResponse.json({ reply: "Error processing request." }, { status: 500 });
  }
}
