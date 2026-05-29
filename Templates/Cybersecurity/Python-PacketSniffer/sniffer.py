"""
sniffer.py — Real-time packet capture and protocol analysis using Scapy

Captures network packets on a specified interface, dispatches them to the
analyzer module for protocol decoding, and prints a summary to the console.
"""

from scapy.all import sniff, IP, TCP, UDP, ICMP, Ether
from analyzer import analyze_packet

# Network interface to listen on (None defaults to first usable interface)
INTERFACE = None
# Number of packets to capture (0 means infinite)
PACKET_COUNT = 50


def packet_callback(packet):
    """
    Callback invoked by Scapy for each captured packet.
    Extracts the IP layer if present and delegates to the analyzer.
    """
    if Ether in packet:
        src_mac = packet[Ether].src
        dst_mac = packet[Ether].dst
    else:
        src_mac = dst_mac = "N/A"

    summary = {
        "src_mac": src_mac,
        "dst_mac": dst_mac,
        "length": len(packet),
        "raw": packet
    }

    if IP in packet:
        ip = packet[IP]
        summary["src_ip"] = ip.src
        summary["dst_ip"] = ip.dst
        summary["protocol"] = ip.proto

        # Identify transport-layer protocol
        if TCP in packet:
            tcp = packet[TCP]
            summary["transport"] = "TCP"
            summary["sport"] = tcp.sport
            summary["dport"] = tcp.dport
            summary["flags"] = tcp.flags
        elif UDP in packet:
            udp = packet[UDP]
            summary["transport"] = "UDP"
            summary["sport"] = udp.sport
            summary["dport"] = udp.dport
        elif ICMP in packet:
            summary["transport"] = "ICMP"
    else:
        summary["src_ip"] = "N/A"
        summary["dst_ip"] = "N/A"
        summary["protocol"] = 0

    # Hand off to the protocol analyzer for in-depth parsing
    details = analyze_packet(summary)
    print(details)


def start_sniffer(interface=None, count=50):
    """
    Start capturing packets on the given interface.

    Args:
        interface: Network interface name (e.g. "eth0", "wlan0").
        count:     Number of packets to capture before stopping.
    """
    print(f"[*] Starting sniffer on {interface or 'default'} (count={count})...")
    sniff(iface=interface, prn=packet_callback, store=False, count=count)


if __name__ == "__main__":
    start_sniffer(interface=INTERFACE, count=PACKET_COUNT)
