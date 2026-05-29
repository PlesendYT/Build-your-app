# C Network Scanner

A raw-socket-based network scanner written in C for Linux. Discovers live hosts on the local network via ARP requests and scans TCP ports using multi-threaded connect probes.

## Features

- **ARP host discovery** — broadcasts ARP requests and collects replies to find active IPs on the LAN.
- **Concurrent TCP port scanning** — divides the port range across 4 worker threads.
- **Formatted report output** — prints alive hosts and their open ports to stdout.

## Build & Run

```bash
make
sudo ./scanner 192.168.1.0/24 1 1024
```

The binary requires root privileges to create raw and raw packet sockets.
