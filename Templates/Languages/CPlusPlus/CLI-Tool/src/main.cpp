// Modern C++ CLI tool with cxxopts argument parsing and JSON config support.
#include <cli.hpp>
#include <cxxopts.hpp>
#include <filesystem>
#include <fstream>
#include <iostream>
#include <nlohmann/json.hpp>

namespace fs = std::filesystem;
using json = nlohmann::json;

// Entry point: parse arguments, load config, execute subcommand.
auto main(int argc, char* argv[]) -> int {
    cxxopts::Options options("cli-tool", "A modern C++ CLI tool");

    options.add_options()
        ("c,config", "Config file", cxxopts::value<std::string>())
        ("v,verbose", "Verbose output")
        ("command", "Command to execute", cxxopts::value<std::string>())
        ("input", "Input file", cxxopts::value<std::string>())
        ("output", "Output file", cxxopts::value<std::string>())
        ("h,help", "Print help");

    options.parse_positional({"command", "input", "output"});

    try {
        auto result = options.parse(argc, argv);

        if (result.count("help")) {
            std::cout << options.help() << "\n";
            return 0;
        }

        if (result.count("config")) {
            auto cfg_path = fs::path{result["config"].as<std::string>()};
            if (!fs::exists(cfg_path)) {
                std::cerr << "Config not found: " << cfg_path << "\n";
                return 1;
            }
            std::ifstream file{cfg_path};
            auto config = json::parse(file);
            if (result.count("verbose")) {
                std::cout << "Loaded config: " << config.dump(2) << "\n";
            }
        }

        auto command = result["command"].as<std::string>();

        if (command == "process") {
            if (!result.count("input")) {
                std::cerr << "Input file required\n";
                return 1;
            }
            auto input = fs::path{result["input"].as<std::string>()};
            std::ifstream file{input};
            if (!file) {
                std::cerr << "Cannot open: " << input << "\n";
                return 1;
            }
            std::string content((std::istreambuf_iterator<char>(file)),
                                 std::istreambuf_iterator<char>());

            if (result.count("output")) {
                auto output = fs::path{result["output"].as<std::string>()};
                std::ofstream out{output};
                out << "Processed " << content.size() << " bytes\n";
            } else {
                std::cout << "Processed " << content.size() << " bytes\n";
            }
        } else if (command == "status") {
            std::cout << "cli-tool v" << PROJECT_VER << "\n";
            std::cout << "Host: " <<
    #ifdef __linux__
                "Linux"
    #elif _WIN32
                "Windows"
    #elif __APPLE__
                "macOS"
    #else
                "Unknown"
    #endif
                << "\n";
        } else {
            std::cerr << "Unknown command: " << command << "\n";
            return 1;
        }
    } catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << "\n";
        return 1;
    }
    return 0;
}
