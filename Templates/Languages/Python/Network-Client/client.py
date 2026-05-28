"""TCP and HTTP client examples."""

from __future__ import annotations

import socket
from urllib.request import urlopen


def tcp_client(host: str = "127.0.0.1", port: int = 8888) -> None:
    """Connect to a TCP echo server."""
    with socket.create_connection((host, port), timeout=5) as sock:
        sock.sendall(b"Hello, server!")
        response = sock.recv(1024)
        print(f"TCP response: {response.decode()}")


def http_client(url: str = "http://127.0.0.1:8000") -> None:
    """Fetch a URL via HTTP GET."""
    try:
        with urlopen(url, timeout=5) as response:
            body = response.read().decode()
            print(f"HTTP response ({response.status}):")
            print(body[:500])
    except Exception as e:
        print(f"HTTP error: {e}")


def main() -> None:
    print("Choose client type:")
    print("1. TCP Client (connect to localhost:8888)")
    print("2. HTTP Client (fetch localhost:8000)")
    choice = input("> ").strip()

    if choice == "1":
        tcp_client()
    elif choice == "2":
        http_client()
    else:
        print("Invalid choice.")


if __name__ == "__main__":
    main()
