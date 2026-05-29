// CLI tool shared types: error codes, result alias, and config struct.
#pragma once
#include <string>
#include <string_view>
#include <expected>

// Error codes for CLI operations.
enum class CliError {
    FileNotFound,
    ParseError,
    UnknownCommand,
};

// Result type alias using std::expected.
template<typename T>
using CliResult = std::expected<T, CliError>;

// Application configuration loaded from a config file.
struct Config {
    std::string name;
    std::string version;
    int threads = 1;
    int timeout = 30;
};
