"""
IRC bot template using raw sockets.
Connects to a server, joins a channel, and responds to !commands.
"""

import socket
import config


def send_raw(sock: socket.socket, data: str):
    """Send a raw IRC message with CRLF termination."""
    sock.send(f"{data}\r\n".encode("utf-8"))


def send_privmsg(sock: socket.socket, target: str, message: str):
    """Send a PRIVMSG to a channel or user."""
    send_raw(sock, f"PRIVMSG {target} :{message}")


def handle_message(sock: socket.socket, user: str, target: str, message: str):
    """Route chat commands to appropriate handlers."""
    if not message.startswith("!"):
        return
    cmd, *args = message[1:].split()
    if cmd == "ping":
        send_privmsg(sock, target, f"{user}: Pong!")
    elif cmd == "hello":
        send_privmsg(sock, target, f"Hello, {user}!")
    elif cmd == "help":
        send_privmsg(sock, target, f"{user}: Commands: !ping, !hello, !help")


def parse_irc(data: str):
    """Parse a raw IRC message and return (user, target, message) or None."""
    if " PRIVMSG " not in data:
        return None
    parts = data.split(" PRIVMSG ", 1)
    if len(parts) < 2:
        return None
    user_part = parts[0].split("!")[0].lstrip(":")
    rest = parts[1].split(" :", 1)
    target = rest[0]
    msg = rest[1].strip() if len(rest) > 1 else ""
    return user_part, target, msg


def main():
    """Connect to IRC, join channel, and enter the message loop."""
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((config.SERVER, config.PORT))
    send_raw(sock, f"NICK {config.NICKNAME}")
    send_raw(sock, f"USER {config.NICKNAME} 0 * :Python IRC Bot")
    send_raw(sock, f"JOIN {config.CHANNEL}")
    print(f"Connected to {config.SERVER} as {config.NICKNAME}")

    buffer = ""
    while True:
        try:
            buffer += sock.recv(2048).decode("utf-8", errors="replace")
        except (ConnectionResetError, OSError):
            print("Connection lost.")
            break
        while "\r\n" in buffer:
            line, buffer = buffer.split("\r\n", 1)
            print(f"< {line}")
            if "PING :" in line:
                send_raw(sock, f"PONG :{line.split('PING :')[1]}")
            parsed = parse_irc(line)
            if parsed:
                user, target, msg = parsed
                handle_message(sock, user, target, msg)

    sock.close()


if __name__ == "__main__":
    main()
