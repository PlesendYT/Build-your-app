// TCP echo server (C++) - accepts clients and echoes their messages.
#include <algorithm>
#include <csignal>
#include <cstring>
#include <iostream>
#include <memory>
#include <sys/socket.h>
#include <sys/types.h>
#include <netinet/in.h>
#include <unistd.h>
#include <vector>

// TCP server: binds, listens, accepts clients, echoes data back.
class TCPServer {
public:
    // Bind and listen on the given port.
    explicit TCPServer(uint16_t port) {
        server_fd_ = socket(AF_INET, SOCK_STREAM, 0);
        if (server_fd_ < 0) throw std::runtime_error("socket() failed");

        int opt = 1;
        setsockopt(server_fd_, SOL_SOCKET, SO_REUSEADDR, &opt, sizeof(opt));

        sockaddr_in addr{};
        addr.sin_family = AF_INET;
        addr.sin_addr.s_addr = INADDR_ANY;
        addr.sin_port = htons(port);

        if (bind(server_fd_, (sockaddr*)&addr, sizeof(addr)) < 0) {
            close(server_fd_);
            throw std::runtime_error("bind() failed");
        }

        if (listen(server_fd_, 5) < 0) {
            close(server_fd_);
            throw std::runtime_error("listen() failed");
        }

        std::cout << "Server listening on port " << port << "\n";
    }

    ~TCPServer() {
        if (server_fd_ >= 0) close(server_fd_);
    }

    // Accept and handle clients until stopped.
    void run() {
        while (running_) {
            sockaddr_in client_addr{};
            socklen_t addr_len = sizeof(client_addr);
            int client = accept(server_fd_, (sockaddr*)&client_addr, &addr_len);
            if (client < 0) {
                if (running_) std::cerr << "accept() failed\n";
                continue;
            }

            std::cout << "Client connected\n";
            handle_client(client);
            close(client);
            std::cout << "Client disconnected\n";
        }
    }

    // Signal the server to stop accepting.
    void stop() { running_ = false; }

private:
    // Read from client and echo data back.
    void handle_client(int client_fd) {
        char buffer[4096];
        std::string response;

        while (true) {
            auto bytes = read(client_fd, buffer, sizeof(buffer) - 1);
            if (bytes <= 0) break;

            buffer[bytes] = '\0';
            std::cout << "Received: " << buffer;

            const char* msg = "Echo: ";
            write(client_fd, msg, strlen(msg));
            write(client_fd, buffer, bytes);
        }
    }

    int server_fd_ = -1;
    bool running_ = true;
};

// Global pointer for signal handler access.
static TCPServer* global_server = nullptr;

// Signal handler: gracefully stop the server.
void handle_signal(int) {
    if (global_server) global_server->stop();
}

// Entry point: create server, register signal handlers, run.
int main(int argc, char* argv[]) {
    auto port = static_cast<uint16_t>(argc > 1 ? std::atoi(argv[1]) : 8080);

    try {
        TCPServer server{port};
        global_server = &server;
        std::signal(SIGINT, handle_signal);
        std::signal(SIGTERM, handle_signal);
        server.run();
    } catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << "\n";
        return 1;
    }
    return 0;
}
