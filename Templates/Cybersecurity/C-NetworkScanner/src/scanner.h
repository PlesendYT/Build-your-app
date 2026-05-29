// scanner.h — Header for the C network scanner library
// Provides ARP scanning, TCP port scanning, and utility types

#ifndef SCANNER_H
#define SCANNER_H

#include <stdint.h>
#include <netinet/in.h>

// Maximum number of ports or hosts we support in one scan batch
#define MAX_TARGETS 256
#define MAX_PORTS   1024
#define TIMEOUT_SEC 2

// Result of scanning a single host
typedef struct {
    char ip[16];             // Dotted-decimal IP string
    int reachable;           // 1 if host responded, 0 otherwise
    int open_ports[MAX_PORTS];
    int port_count;
} ScanResult;

// Perform an ARP scan on a CIDR subnet (e.g. "192.168.1.0/24")
// Returns an array of ScanResult structs, one per discovered host.
// The caller must free the returned pointer.
ScanResult* arp_scan(const char* subnet, int* out_count);

// TCP connect scan against a single host on the given port range.
// Fills scan_result->open_ports with responsive ports.
void port_scan(ScanResult* result, int start_port, int end_port);

// Print a formatted scan report to stdout
void print_report(ScanResult* results, int count);

#endif
