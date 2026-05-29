// SFML coin-collection game - collect yellow coins with keyboard controls (C++).
#include <SFML/Graphics.hpp>
#include <array>
#include <random>
#include <vector>

// A collectible coin with a circular shape.
struct Coin {
    sf::CircleShape shape;
};

// Entry point: create window, spawn coins, run game loop.
auto main() -> int {
    sf::RenderWindow window{sf::VideoMode{{800, 600}}, "SFML Game"};
    window.setFramerateLimit(60);

    sf::CircleShape player(20.f);
    player.setFillColor(sf::Color::Blue);
    player.setPosition({400.f, 300.f});

    std::vector<Coin> coins;
    std::mt19937 gen{std::random_device{}()};
    std::uniform_real_distribution pos_dist{0.f, 750.f};

    for (int i = 0; i < 10; ++i) {
        Coin coin;
        coin.shape = sf::CircleShape(10.f);
        coin.shape.setFillColor(sf::Color::Yellow);
        coin.shape.setPosition({pos_dist(gen), pos_dist(gen)});
        coins.push_back(std::move(coin));
    }

    int score = 0;
    sf::Font font;
    font.openFromFile("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf");

    sf::Text score_text{font};
    score_text.setCharacterSize(24);
    score_text.setFillColor(sf::Color::White);
    score_text.setPosition({10.f, 10.f});

    while (window.isOpen()) {
        while (auto event = window.pollEvent()) {
            if (event->is<sf::Event::Closed>()) {
                window.close();
            }
        }

        float speed = 5.f;
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::Left))
            player.move({-speed, 0.f});
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::Right))
            player.move({speed, 0.f});
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::Up))
            player.move({0.f, -speed});
        if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::Down))
            player.move({0.f, speed});

        auto player_bounds = player.getGlobalBounds();

        std::erase_if(coins, [&](const Coin& coin) {
            if (coin.shape.getGlobalBounds().findIntersection(player_bounds)) {
                score += 10;
                return true;
            }
            return false;
        });

        window.clear(sf::Color::Black);
        window.draw(player);
        for (const auto& coin : coins) {
            window.draw(coin.shape);
        }
        score_text.setString("Score: " + std::to_string(score));
        window.draw(score_text);

        if (coins.empty()) {
            sf::Text win_text{font};
            win_text.setString("You Win!");
            win_text.setCharacterSize(50);
            win_text.setFillColor(sf::Color::Green);
            win_text.setPosition({300.f, 280.f});
            window.draw(win_text);
        }

        window.display();
    }
    return 0;
}
