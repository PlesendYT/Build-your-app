#include <arpa/inet.h>
#include <errno.h>
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

#define BUFFER_SIZE 4096
#define DEFAULT_PORT 8080

static volatile int running = 1;

static void handle_signal(int sig) {
    (void)sig;
    running = 0;
}

static void handle_client(int client_fd) {
    char buffer[BUFFER_SIZE];
    ssize_t bytes;
    char addr_str[INET_ADDRSTRLEN];
    struct sockaddr_in addr;
    socklen_t addr_len = sizeof(addr);

    getpeername(client_fd, (struct sockaddr*)&addr, &addr_len);
    inet_ntop(AF_INET, &addr.sin_addr, addr_str, sizeof(addr_str));
    printf("Client connected: %s:%d\n", addr_str, ntohs(addr.sin_port));

    while ((bytes = read(client_fd, buffer, sizeof(buffer) - 1)) > 0) {
        buffer[bytes] = '\0';
        printf("Received: %s", buffer);

        const char *prefix = "Echo: ";
        write(client_fd, prefix, strlen(prefix));
        write(client_fd, buffer, bytes);
    }

    printf("Client disconnected: %s\n", addr_str);
    close(client_fd);
}

int main(int argc, char *argv[]) {
    int port = DEFAULT_PORT;
    if (argc > 1) port = atoi(argv[1]);

    signal(SIGINT, handle_signal);
    signal(SIGTERM, handle_signal);

    int server_fd = socket(AF_INET, SOCK_STREAM, 0);
    if (server_fd < 0) {
        perror("socket");
        return 1;
    }

    int opt = 1;
    setsockopt(server_fd, SOL_SOCKET, SO_REUSEADDR, &opt, sizeof(opt));

    struct sockaddr_in addr = {0};
    addr.sin_family = AF_INET;
    addr.sin_addr.s_addr = INADDR_ANY;
    addr.sin_port = htons(port);

    if (bind(server_fd, (struct sockaddr*)&addr, sizeof(addr)) < 0) {
        perror("bind");
        close(server_fd);
        return 1;
    }

    if (listen(server_fd, 5) < 0) {
        perror("listen");
        close(server_fd);
        return 1;
    }

    printf("Server listening on port %d\n", port);

    while (running) {
        struct sockaddr_in client_addr;
        socklen_t client_len = sizeof(client_addr);
        int client_fd = accept(server_fd, (struct sockaddr*)&client_addr, &client_len);

        if (client_fd < 0) {
            if (running) perror("accept");
            continue;
        }

        handle_client(client_fd);
    }

    printf("Server shutting down\n");
    close(server_fd);
    return 0;
}
