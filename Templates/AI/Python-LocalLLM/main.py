"""Local LLM runner using llama-cpp-python for offline inference."""

from __future__ import annotations

import json
from pathlib import Path
from typing import Any

from llama_cpp import Llama


def load_config(path: str = "config.json") -> dict[str, Any]:
    """Load model configuration from a JSON file."""
    config_file = Path(path)
    if not config_file.exists():
        # Fall back to default configuration if no config file found
        return {
            "model_path": "models/llama-2-7b.Q4_K_M.gguf",
            "n_ctx": 2048,
            "n_threads": 4,
            "max_tokens": 512,
            "temperature": 0.7,
        }
    with open(config_file) as f:
        return json.load(f)


def build_prompt(user_input: str, system_prompt: str | None = None) -> str:
    """Format a prompt with optional system instructions."""
    if system_prompt:
        return f"[INST] <<SYS>>\n{system_prompt}\n<</SYS>>\n\n{user_input} [/INST]"
    return f"[INST] {user_input} [/INST]"


def main() -> None:
    """Load the local LLM and run an interactive prompt loop."""
    config = load_config()

    # Initialize the LLM with the specified model and parameters
    llm = Llama(
        model_path=config["model_path"],
        n_ctx=config["n_ctx"],
        n_threads=config["n_threads"],
        verbose=False,
    )

    system_prompt = "You are a helpful, concise assistant."
    print("Local LLM ready. Type 'quit' to exit.\n")

    while True:
        # Read user input from the command line
        user_input = input(">>> ")
        if user_input.lower() in {"quit", "exit"}:
            break

        # Format the prompt and generate a response
        prompt = build_prompt(user_input, system_prompt)
        response = llm(
            prompt,
            max_tokens=config["max_tokens"],
            temperature=config["temperature"],
            echo=False,
        )

        # Print the generated text
        print(response["choices"][0]["text"].strip())
        print()


if __name__ == "__main__":
    main()
