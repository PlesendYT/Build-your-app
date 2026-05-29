# analyzer.py — Deep protocol inspection and packet analysis module
# Receives packet summaries from the sniffer and returns human-readable reports

import datetime


# Well-known service ports for protocol identification
KNOWN_PORTS = {
    22: "SSH", 23: "Telnet", 25: "SMTP", 53: "DNS",
    80: "HTTP", 110: "POP3", 143: "IMAP", 443: "HTTPS",
    3306: "MySQL", 8080: "HTTP-Alt"
}


def identify_service(port):
    """Return the service name for a well-known port, or 'Unknown'."""
    return KNOWN_PORTS.get(port, "Unknown")


def analyze_packet(summary):
    """
    Build a formatted analysis string from the raw packet summary dict.

    Expected keys: src_ip, dst_ip, src_mac, dst_mac, transport, sport,
                   dport, flags, protocol, length
    """
    timestamp = datetime.datetime.now().strftime("%H:%M:%S.%f")[:-3]
    src = f"{summary.get('src_ip', '?')}:{summary.get('sport', '?')}"
    dst = f"{summary.get('dst_ip', '?')}:{summary.get('dport', '?')}"
    proto = summary.get("transport", f"IP-{summary.get('protocol', '?')}")
    length = summary.get("length", 0)
    service = identify_service(summary.get("dport", 0))

    # Build a single-line packet summary
    line = f"[{timestamp}] {proto:5s} {src:22s} -> {dst:22s} "
    line += f"| len={length:5d} | svc={service:12s}"

    # Append TCP flags for TCP segments
    flags = summary.get("flags")
    if flags:
        line += f" | flags={flags:2s}"

    return line
