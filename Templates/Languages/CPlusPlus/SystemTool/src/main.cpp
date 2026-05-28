#include <algorithm>
#include <chrono>
#include <csignal>
#include <cstring>
#include <fstream>
#include <iostream>
#include <memory>
#include <string>
#include <thread>
#include <vector>

#ifdef __linux__
#include <unistd.h>
#include <sys/sysinfo.h>
#include <dirent.h>
#endif

auto read_file(const std::string& path) -> std::string {
    std::ifstream file{path};
    if (!file) return {};
    std::string content, line;
    while (std::getline(file, line)) {
        content += line + "\n";
    }
    return content;
}

auto get_cpu_count() -> long {
#ifdef _SC_NPROCESSORS_ONLN
    return sysconf(_SC_NPROCESSORS_ONLN);
#else
    return 1;
#endif
}

auto get_memory_info() -> std::pair<unsigned long long, unsigned long long> {
    std::ifstream meminfo{"/proc/meminfo"};
    if (!meminfo) return {0, 0};

    unsigned long long total = 0, available = 0;
    std::string line;
    while (std::getline(meminfo, line)) {
        if (line.starts_with("MemTotal:")) {
            std::sscanf(line.c_str(), "MemTotal: %llu", &total);
        } else if (line.starts_with("MemAvailable:")) {
            std::sscanf(line.c_str(), "MemAvailable: %llu", &available);
        }
    }
    return {total, available};
}

auto format_bytes(double bytes) -> std::string {
    const char* units[] = {"B", "KB", "MB", "GB", "TB"};
    int unit = 0;
    while (bytes > 1024 && unit < 4) {
        bytes /= 1024;
        ++unit;
    }
    char buf[64];
    std::snprintf(buf, sizeof(buf), "%.2f %s", bytes, units[unit]);
    return {buf};
}

auto print_cpu_info() -> void {
    std::cout << "=== CPU ===\n";
    std::cout << "Cores: " << get_cpu_count() << "\n";

    std::ifstream cpuinfo{"/proc/cpuinfo"};
    if (cpuinfo) {
        std::string line;
        while (std::getline(cpuinfo, line)) {
            if (line.starts_with("model name")) {
                auto pos = line.find(':');
                if (pos != std::string::npos) {
                    std::cout << "Model:" << line.substr(pos + 1) << "\n";
                }
                break;
            }
        }
    }
}

auto print_memory_info() -> void {
    auto [total, available] = get_memory_info();
    std::cout << "=== Memory ===\n";
    std::cout << "Total:     " << format_bytes(static_cast<double>(total) * 1024) << "\n";
    std::cout << "Available: " << format_bytes(static_cast<double>(available) * 1024) << "\n";
}

auto main() -> int {
    std::cout << "System Tool v1.0\n\n";
    print_cpu_info();
    std::cout << "\n";
    print_memory_info();
    return 0;
}
