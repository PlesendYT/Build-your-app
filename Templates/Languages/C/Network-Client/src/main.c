// TCP network client - connects to a server, sends user input, echoes response.
#include <arpa/inet.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

#define BUFFER_SIZE 4096

// Entry point: connect to host:port, then run interactive send/receive loop.
int main(int argc, char *argv[]) {
    const char *host = argc > 1 ? argv[1] : "127.0.0.1";
    int port = argc > 2 ? atoi(argv[2]) : 8080;

    // Create TCP socket.
    int fd = socket(AF_INET, SOCK_STREAM, 0);
    if (fd < 0) {
        perror("socket");
        return 1;
    }

    struct sockaddr_in addr = {0};
    addr.sin_family = AF_INET;
    addr.sin_port = htons(port);
    if (inet_pton(AF_INET, host, &addr.sin_addr) <= 0) {
        fprintf(stderr, "Invalid address: %s\n", host);
        close(fd);
        return 1;
    }

    // Connect to the remote host.
    if (connect(fd, (struct sockaddr*)&addr, sizeof(addr)) < 0) {
        perror("connect");
        close(fd);
        return 1;
    }

    printf("Connected to %s:%d\n", host, port);

    // Interactive I/O loop: read user input, send to server, print response.
    char line[BUFFER_SIZE];
    while (printf("> "), fgets(line, sizeof(line), stdin)) {
        write(fd, line, strlen(line));

        char buffer[BUFFER_SIZE] = {0};
        ssize_t bytes = read(fd, buffer, sizeof(buffer) - 1);
        if (bytes <= 0) {
            printf("Connection closed\n");
            break;
        }
        printf("%s", buffer);
    }

    close(fd);
    return 0;
}
