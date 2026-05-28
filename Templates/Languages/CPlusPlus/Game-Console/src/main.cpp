#include <algorithm>
#include <cstdlib>
#include <ctime>
#include <iostream>
#include <string>
#include <vector>

enum class Choice { Rock, Paper, Scissors };

auto to_choice(const std::string& s) -> std::optional<Choice> {
    if (s == "rock" || s == "r") return Choice::Rock;
    if (s == "paper" || s == "p") return Choice::Paper;
    if (s == "scissors" || s == "s") return Choice::Scissors;
    return std::nullopt;
}

auto to_string(Choice c) -> std::string_view {
    using namespace std::string_view_literals;
    switch (c) {
        case Choice::Rock: return "Rock"sv;
        case Choice::Paper: return "Paper"sv;
        case Choice::Scissors: return "Scissors"sv;
    }
    return "?"sv;
}

auto determine_winner(Choice player, Choice computer) -> int {
    if (player == computer) return 0;
    if ((player == Choice::Rock && computer == Choice::Scissors) ||
        (player == Choice::Paper && computer == Choice::Rock) ||
        (player == Choice::Scissors && computer == Choice::Paper)) {
        return 1;
    }
    return -1;
}

auto main() -> int {
    std::mt19937 gen{std::random_device{}()};
    std::uniform_int_distribution dist{0, 2};

    std::cout << "Rock-Paper-Scissors!\n";
    std::cout << "Choices: rock (r), paper (p), scissors (s), quit (q)\n";

    while (true) {
        std::cout << "> ";
        std::string input;
        if (!(std::cin >> input)) break;

        if (input == "q" || input == "quit") break;

        auto player = to_choice(input);
        if (!player) {
            std::cout << "Invalid choice.\n";
            continue;
        }

        auto computer = static_cast<Choice>(dist(gen));
        std::cout << "You: " << to_string(*player)
                  << " vs Computer: " << to_string(computer) << "\n";

        auto result = determine_winner(*player, computer);
        if (result == 0) {
            std::cout << "Draw!\n";
        } else if (result == 1) {
            std::cout << "You win!\n";
        } else {
            std::cout << "Computer wins!\n";
        }
    }
    return 0;
}
