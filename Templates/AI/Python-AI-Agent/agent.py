"""AI agent with function calling: tool definitions, routing, and execution loop."""

from __future__ import annotations

import json
import re
from typing import Any

import requests

# Registry mapping tool names to their callable implementations
TOOL_REGISTRY: dict[str, callable] = {}


def register_tool(func: callable) -> callable:
    """Decorator to register a function as an available tool."""
    TOOL_REGISTRY[func.__name__] = func
    return func


@register_tool
def get_weather(city: str) -> str:
    """Get the current weather for a given city."""
    # Import here to avoid circular dependency
    from tools.weather import get_weather as _weather
    return _weather(city)


@register_tool
def calculator(expression: str) -> str:
    """Evaluate a mathematical expression."""
    from tools.calculator import calculate as _calc
    return _calc(expression)


def dispatch_tool(name: str, args: dict[str, Any]) -> str:
    """Route a tool call to the registered implementation."""
    if name not in TOOL_REGISTRY:
        return f"Error: unknown tool '{name}'"
    try:
        return TOOL_REGISTRY[name](**args)
    except Exception as e:
        return f"Error executing {name}: {e}"


def agent_loop(user_input: str) -> str:
    """Parse intent, call tools, and return a formatted response."""
    # Simple intent parser using regex patterns
    # In production, replace this with an LLM call for function calling

    # Weather pattern: "weather in <city>" or "what's the weather in <city>"
    weather_match = re.search(r"weather (?:in|for) (\w+)", user_input, re.IGNORECASE)
    if weather_match:
        result = dispatch_tool("get_weather", {"city": weather_match.group(1)})
        return f"[Weather] {result}"

    # Calculator pattern: "calculate <expr>" or "what is <expr>"
    calc_match = re.search(r"(?:calculate|what is|compute) (.+)", user_input, re.IGNORECASE)
    if calc_match:
        result = dispatch_tool("calculator", {"expression": calc_match.group(1)})
        return f"[Calculator] {result}"

    return "I'm not sure how to help with that. Try 'weather in London' or 'calculate 2 + 2'."


def main() -> None:
    """Run the AI agent interaction loop."""
    print("AI Agent ready. Available tools: get_weather, calculator\n")

    while True:
        user_input = input("You: ")
        if user_input.lower() in {"quit", "exit"}:
            break

        response = agent_loop(user_input)
        print(f"Agent: {response}\n")


if __name__ == "__main__":
    main()
