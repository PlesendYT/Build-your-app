#include <arpa/inet.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/socket.h>
#include <unistd.h>

int main(void) {
    int s = socket(AF_INET, SOCK_STREAM, 0);
    int opt = 1; setsockopt(s, SOL_SOCKET, SO_REUSEADDR, &opt, sizeof opt);
    struct sockaddr_in a = { .sin_family = AF_INET, .sin_port = htons(9000), .sin_addr.s_addr = INADDR_ANY };
    bind(s, (struct sockaddr*)&a, sizeof a);
    listen(s, 8);
    puts("listening on :9000");
    for (;;) {
        int c = accept(s, NULL, NULL);
        char buf[1024]; ssize_t n;
        while ((n = read(c, buf, sizeof buf)) > 0) write(c, buf, n);
        close(c);
    }
}