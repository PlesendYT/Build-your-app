#pragma once
#include <string>
#include <string_view>
#include <expected>

enum class CliError {
    FileNotFound,
    ParseError,
    UnknownCommand,
};

template<typename T>
using CliResult = std::expected<T, CliError>;

struct Config {
    std::string name;
    std::string version;
    int threads = 1;
    int timeout = 30;
};
