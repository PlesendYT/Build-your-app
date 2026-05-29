// SFML GUI template - window with shape and text (C++).
#include <SFML/Graphics.hpp>
#include <array>
#include <string>

// Entry point: create window, render shape and "Hello, SFML!" text.
auto main() -> int {
    sf::RenderWindow window{sf::VideoMode{{800, 600}}, "SFML Template"};
    window.setFramerateLimit(60);

    sf::Font font;
    if (!font.openFromFile("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf")) {
        return 1;
    }

    sf::Text text{font};
    text.setString("Hello, SFML!");
    text.setCharacterSize(30);
    text.setFillColor(sf::Color::White);
    text.setPosition({300.f, 280.f});

    sf::CircleShape shape{50.f};
    shape.setFillColor(sf::Color::Blue);
    shape.setPosition({100.f, 100.f});

    while (window.isOpen()) {
        while (auto event = window.pollEvent()) {
            if (event->is<sf::Event::Closed>()) {
                window.close();
            }
        }

        window.clear(sf::Color::Black);
        window.draw(shape);
        window.draw(text);
        window.display();
    }
    return 0;
}
