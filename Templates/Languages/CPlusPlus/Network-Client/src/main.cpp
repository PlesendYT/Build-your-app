// TCP network client (C++) - connects to server and sends user messages.
#include <cstring>
#include <iostream>
#include <string>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>

// TCP client: connects, sends messages, receives responses.
class TCPClient {
public:
    // Connect to host:port; throws on failure.
    TCPClient(const std::string& host, uint16_t port) {
        fd_ = socket(AF_INET, SOCK_STREAM, 0);
        if (fd_ < 0) throw std::runtime_error("socket() failed");

        sockaddr_in addr{};
        addr.sin_family = AF_INET;
        addr.sin_port = htons(port);
        if (inet_pton(AF_INET, host.c_str(), &addr.sin_addr) <= 0) {
            close(fd_);
            throw std::runtime_error("Invalid address");
        }

        if (connect(fd_, (sockaddr*)&addr, sizeof(addr)) < 0) {
            close(fd_);
            throw std::runtime_error("connect() failed");
        }

        std::cout << "Connected to " << host << ":" << port << "\n";
    }

    // Close socket on destruction.
    ~TCPClient() {
        if (fd_ >= 0) close(fd_);
    }

    // Send a message over the TCP connection.
    void send_message(const std::string& msg) {
        send(fd_, msg.c_str(), msg.size(), 0);
    }

    // Receive a response from the server.
    std::string receive() {
        char buffer[4096];
        auto bytes = recv(fd_, buffer, sizeof(buffer) - 1, 0);
        if (bytes > 0) {
            buffer[bytes] = '\0';
            return {buffer};
        }
        return {};
    }

    // Interactive loop: read stdin, send to server, print response.
    void interactive() {
        std::string line;
        while (std::getline(std::cin, line)) {
            if (line == "quit") break;
            send_message(line + "\n");
            auto response = receive();
            if (response.empty()) {
                std::cout << "Connection closed\n";
                break;
            }
            std::cout << "Server: " << response;
        }
    }

private:
    int fd_ = -1;
};

// Entry point: parse args, create client, run interactive session.
int main(int argc, char* argv[]) {
    auto host = argc > 1 ? argv[1] : "127.0.0.1";
    auto port = static_cast<uint16_t>(argc > 2 ? std::atoi(argv[2]) : 8080);

    try {
        TCPClient client{host, port};
        client.interactive();
    } catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << "\n";
        return 1;
    }
    return 0;
}
