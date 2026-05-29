"""Simulated weather tool for the AI agent."""

from __future__ import annotations

import random

# Simulated weather database for demonstration purposes
# Replace this with a real API call in production
_WEATHER_DATA: dict[str, dict[str, str | float]] = {
    "london": {"condition": "rainy", "temperature": 14.0},
    "paris": {"condition": "cloudy", "temperature": 18.0},
    "tokyo": {"condition": "sunny", "temperature": 22.0},
    "newyork": {"condition": "clear", "temperature": 20.0},
    "berlin": {"condition": "partly_cloudy", "temperature": 16.0},
}


def get_weather(city: str) -> str:
    """Return the current weather for a given city as a formatted string."""
    city_key = city.lower().replace(" ", "")
    if city_key in _WEATHER_DATA:
        data = _WEATHER_DATA[city_key]
        return f"{data['condition']}, {data['temperature']}°C"
    # Simulate a random weather result for unknown cities
    conditions = ["sunny", "rainy", "cloudy", "windy"]
    return f"{random.choice(conditions)}, {random.uniform(5, 30):.1f}°C"
