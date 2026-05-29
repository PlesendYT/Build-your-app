"""AI chatbot using the transformers library with a conversation loop."""

from __future__ import annotations

from transformers import pipeline, Conversation

# Load a conversational AI pipeline using a pre-trained model
# Uses the DialoGPT-medium model for natural dialogue
chatbot = pipeline("conversational", model="microsoft/DialoGPT-medium")


def chat_loop() -> None:
    """Run an interactive conversation loop with the AI."""
    # Track conversation history
    conversation = Conversation()

    print("AI Chatbot ready! Type 'quit' to exit.\n")

    while True:
        # Get user input from the console
        user_input = input("You: ")
        if user_input.lower() in {"quit", "exit"}:
            print("Goodbye!")
            break

        # Add the new user input to the conversation context
        conversation.add_user_input(user_input)

        # Generate a response using the transformer model
        conversation = chatbot(conversation)

        # Extract and display the AI's reply
        response = conversation.generated_responses[-1]
        print(f"AI: {response}\n")


if __name__ == "__main__":
    """Entry point - start the chatbot conversation loop."""
    chat_loop()
