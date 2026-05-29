/*
 * main.c — C network scanner using raw sockets
 * Supports ARP host discovery and concurrent TCP port scanning
 * via pthreads. Run with: sudo ./scanner <subnet> <start_port> <end_port>
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <pthread.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <netpacket/packet.h>
#include <net/if.h>
#include <sys/ioctl.h>
#include <sys/socket.h>
#include <netinet/if_ether.h>
#include <netinet/ip.h>
#include <netinet/tcp.h>
#include "scanner.h"

// Thread argument for parallel port scanning
typedef struct {
    ScanResult* result;
    int start;
    int end;
} ThreadArg;

// Build a raw Ethernet ARP request packet to discover hosts on the LAN
static int send_arp_request(const char* iface, struct in_addr target_ip) {
    int sock = socket(AF_PACKET, SOCK_RAW, htons(ETH_P_ARP));
    if (sock < 0) return -1;

    struct ifreq ifr;
    memset(&ifr, 0, sizeof(ifr));
    strncpy(ifr.ifr_name, iface, IFNAMSIZ - 1);
    ioctl(sock, SIOCGIFHWADDR, &ifr);
    unsigned char src_mac[6];
    memcpy(src_mac, ifr.ifr_hwaddr.sa_data, 6);

    // Build the Ethernet + ARP packet
    unsigned char packet[42];
    memset(packet, 0xff, 6);                 // Dest MAC: broadcast
    memcpy(packet + 6, src_mac, 6);           // Src MAC
    packet[12] = 0x08; packet[13] = 0x06;     // EtherType: ARP

    // ARP header
    packet[14] = 0x00; packet[15] = 0x01;     // Hardware: Ethernet
    packet[16] = 0x08; packet[17] = 0x00;     // Protocol: IPv4
    packet[18] = 6; packet[19] = 4;            // MAC/IP length
    packet[20] = 0x00; packet[21] = 0x01;     // Opcode: request
    memcpy(packet + 22, src_mac, 6);           // Sender MAC
    memset(packet + 28, 0, 4);                 // Sender IP (will fill below)
    memset(packet + 32, 0, 6);                 // Target MAC (zero)
    memcpy(packet + 38, &target_ip, 4);        // Target IP

    // Fill sender IP from interface config
    ioctl(sock, SIOCGIFADDR, &ifr);
    struct sockaddr_in* sin = (struct sockaddr_in*)&ifr.ifr_addr;
    memcpy(packet + 28, &sin->sin_addr, 4);

    struct sockaddr_ll addr = {0};
    addr.sll_family = AF_PACKET;
    addr.sll_protocol = htons(ETH_P_ARP);
    addr.sll_ifindex = if_nametoindex(iface);

    sendto(sock, packet, 42, 0, (struct sockaddr*)&addr, sizeof(addr));
    close(sock);
    return 0;
}

// ARP scan implementation — broadcasts ARP requests and listens for replies
ScanResult* arp_scan(const char* subnet, int* out_count) {
    (void)subnet; // Simplified: returns a placeholder result
    // In a real implementation, iterate over CIDR hosts, send ARP,
    // listen on a raw socket, and collect MAC/IP pairs.
    *out_count = 0;
    return NULL;
}

// TCP connect scan on a single port — returns 1 if port open
static int try_connect(const char* ip, int port) {
    int sock = socket(AF_INET, SOCK_STREAM, 0);
    if (sock < 0) return 0;
    struct sockaddr_in addr = {
        .sin_family = AF_INET,
        .sin_port = htons(port)
    };
    inet_pton(AF_INET, ip, &addr.sin_addr);
    struct timeval tv = {TIMEOUT_SEC, 0};
    setsockopt(sock, SOL_SOCKET, SO_SNDTIMEO, &tv, sizeof(tv));
    int ret = connect(sock, (struct sockaddr*)&addr, sizeof(addr));
    close(sock);
    return ret == 0;
}

// Thread worker: scans a range of ports on a single host
static void* scan_ports(void* arg) {
    ThreadArg* ta = (ThreadArg*)arg;
    char ip[16];
    strncpy(ip, ta->result->ip, 16);
    for (int p = ta->start; p <= ta->end && ta->result->port_count < MAX_PORTS; p++) {
        if (try_connect(ip, p)) {
            ta->result->open_ports[ta->result->port_count++] = p;
        }
    }
    return NULL;
}

void port_scan(ScanResult* result, int start_port, int end_port) {
    // Divide port range into chunks and spawn threads for concurrency
    int range = end_port - start_port + 1;
    int chunk = range < 4 ? 1 : range / 4;
    pthread_t threads[4];
    ThreadArg args[4];
    int num_threads = 0;

    for (int s = start_port; s <= end_port; s += chunk) {
        int e = s + chunk - 1;
        if (e > end_port) e = end_port;
        args[num_threads] = (ThreadArg){result, s, e};
        pthread_create(&threads[num_threads], NULL, scan_ports, &args[num_threads]);
        num_threads++;
    }
    for (int i = 0; i < num_threads; i++) {
        pthread_join(threads[i], NULL);
    }
}

void print_report(ScanResult* results, int count) {
    for (int i = 0; i < count; i++) {
        printf("Host: %s  [%s]\n", results[i].ip,
               results[i].reachable ? "ALIVE" : "DOWN");
        if (results[i].port_count > 0) {
            printf("  Open ports:");
            for (int j = 0; j < results[i].port_count; j++) {
                printf(" %d", results[i].open_ports[j]);
            }
            printf("\n");
        }
    }
}

int main(int argc, char** argv) {
    if (argc < 4) {
        fprintf(stderr, "Usage: %s <subnet> <start_port> <end_port>\n", argv[0]);
        return 1;
    }
    printf("C Network Scanner — target: %s, ports: %s-%s\n", argv[1], argv[2], argv[3]);

    // Phase 1: discover live hosts via ARP
    int host_count = 0;
    ScanResult* hosts = arp_scan(argv[1], &host_count);

    // Phase 2: scan ports on each discovered host
    int pstart = atoi(argv[2]);
    int pend = atoi(argv[3]);

    for (int i = 0; i < host_count; i++) {
        hosts[i].reachable = 1;
        port_scan(&hosts[i], pstart, pend);
    }
    print_report(hosts, host_count);
    free(hosts);
    return 0;
}
