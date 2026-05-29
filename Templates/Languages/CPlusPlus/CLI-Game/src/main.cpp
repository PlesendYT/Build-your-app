// Number guessing game (C++) - guess a number in a limited number of tries.
#include <algorithm>
#include <chrono>
#include <iostream>
#include <limits>
#include <random>
#include <string>

// Number guessing game: generates a random secret, player tries to guess it.
class NumberGuessingGame {
public:
    // Constructor: picks a random secret between lower and upper.
    NumberGuessingGame(int lower, int upper, int max_attempts)
        : lower_{lower}
        , upper_{upper}
        , max_attempts_{max_attempts} {
        std::random_device rd;
        std::mt19937 gen{rd()};
        std::uniform_int_distribution dist{lower, upper};
        secret_ = dist(gen);
    }

    // Main game loop: prompt, read guesses, give hints.
    auto play() -> void {
        std::cout << "Guess a number between " << lower_ << " and " << upper_
                  << "! You have " << max_attempts_ << " attempts.\n";

        while (attempts_ < max_attempts_) {
            std::cout << "Attempt " << (attempts_ + 1) << ": ";

            int guess;
            if (!(std::cin >> guess)) {
                std::cin.clear();
                std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                std::cout << "Invalid input.\n";
                continue;
            }

            ++attempts_;

            if (guess < secret_) {
                std::cout << "Too low!\n";
            } else if (guess > secret_) {
                std::cout << "Too high!\n";
            } else {
                std::cout << "Correct! You got it in " << attempts_
                          << " attempts!\n";
                return;
            }
        }

        std::cout << "Game over! The number was " << secret_ << ".\n";
    }

private:
    int lower_;
    int upper_;
    int max_attempts_;
    int secret_;
    int attempts_{0};
};

// Entry point: create game and run.
auto main() -> int {
    NumberGuessingGame game{1, 100, 7};
    game.play();
    return 0;
}
