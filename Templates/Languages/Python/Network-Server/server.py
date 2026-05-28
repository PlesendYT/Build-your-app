"""TCP and HTTP server examples."""

from __future__ import annotations

import socket
import threading
from http.server import HTTPServer, SimpleHTTPRequestHandler


def handle_client(conn: socket.socket, addr: tuple[str, int]) -> None:
    """Handle a single TCP client connection."""
    print(f"Connected by {addr}")
    with conn:
        while True:
            data = conn.recv(1024)
            if not data:
                break
            conn.sendall(data)  # Echo
    print(f"Disconnected: {addr}")


def run_tcp_server(host: str = "127.0.0.1", port: int = 8888) -> None:
    """Run a multi-threaded TCP echo server."""
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as server:
        server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        server.bind((host, port))
        server.listen()
        print(f"TCP server listening on {host}:{port}")

        try:
            while True:
                conn, addr = server.accept()
                thread = threading.Thread(
                    target=handle_client, args=(conn, addr), daemon=True,
                )
                thread.start()
        except KeyboardInterrupt:
            print("\nServer shutting down.")


def run_http_server(
    host: str = "127.0.0.1", port: int = 8000,
) -> None:
    """Run a simple HTTP server."""
    server = HTTPServer((host, port), SimpleHTTPRequestHandler)
    print(f"HTTP server listening on http://{host}:{port}")
    try:
        server.serve_forever()
    except KeyboardInterrupt:
        print("\nServer shutting down.")
        server.server_close()


def main() -> None:
    print("Choose server type:")
    print("1. TCP Echo Server (port 8888)")
    print("2. HTTP Server (port 8000)")
    choice = input("> ").strip()

    if choice == "1":
        run_tcp_server()
    elif choice == "2":
        run_http_server()
    else:
        print("Invalid choice.")


if __name__ == "__main__":
    main()
