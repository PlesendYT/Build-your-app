#include <arpa/inet.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/socket.h>
#include <unistd.h>

int main(int argc, char** argv) {
    if (argc < 2) return 1;
    int s = socket(AF_INET, SOCK_STREAM, 0);
    struct sockaddr_in a = { .sin_family = AF_INET, .sin_port = htons(9000) };
    inet_pton(AF_INET, "127.0.0.1", &a.sin_addr);
    connect(s, (struct sockaddr*)&a, sizeof a);
    write(s, argv[1], strlen(argv[1]));
    char buf[1024]; ssize_t n = read(s, buf, sizeof buf - 1);
    if (n > 0) { buf[n] = 0; printf("%s\n", buf); }
    close(s);
    return 0;
}