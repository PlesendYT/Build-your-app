// SDL2 coin-collection game (C++) - collect coins with keyboard controls.
#include <SDL2/SDL.h>
#include <SDL2/SDL_ttf.h>
#include <algorithm>
#include <iostream>
#include <memory>
#include <random>
#include <vector>
#include <string>

// Custom deleters for SDL unique_ptr wrappers.
struct SDLDeleter {
    void operator()(SDL_Window* w) const { SDL_DestroyWindow(w); }
    void operator()(SDL_Renderer* r) const { SDL_DestroyRenderer(r); }
};
using WindowPtr = std::unique_ptr<SDL_Window, SDLDeleter>;
using RendererPtr = std::unique_ptr<SDL_Renderer, SDLDeleter>;

// Entry point: initialize, spawn coins, run game loop.
int main(int argc, char* argv[]) {
    SDL_Init(SDL_INIT_VIDEO);
    TTF_Init();

    WindowPtr window(SDL_CreateWindow("SDL2 Game", SDL_WINDOWPOS_CENTERED,
        SDL_WINDOWPOS_CENTERED, 800, 600, SDL_WINDOW_SHOWN));
    RendererPtr renderer(SDL_CreateRenderer(window.get(), -1,
        SDL_RENDERER_ACCELERATED));

    SDL_Rect player = {400 - 20, 300 - 20, 40, 40};
    int score = 0;

    std::mt19937 gen(std::random_device{}());
    std::uniform_int_distribution<int> xdist(0, 760);
    std::uniform_int_distribution<int> ydist(0, 560);

    std::vector<SDL_Rect> coins;
    for (int i = 0; i < 10; ++i) {
        coins.push_back({xdist(gen), ydist(gen), 20, 20});
    }

    bool running = true;
    auto* font = TTF_OpenFont("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf", 24);

    while (running) {
        SDL_Event e;
        while (SDL_PollEvent(&e)) {
            if (e.type == SDL_QUIT) running = false;
        }

        const Uint8* keys = SDL_GetKeyboardState(nullptr);
        int speed = 5;
        if (keys[SDL_SCANCODE_LEFT])  player.x -= speed;
        if (keys[SDL_SCANCODE_RIGHT]) player.x += speed;
        if (keys[SDL_SCANCODE_UP])    player.y -= speed;
        if (keys[SDL_SCANCODE_DOWN])  player.y += speed;

        SDL_SetRenderDrawColor(renderer.get(), 30, 30, 30, 255);
        SDL_RenderClear(renderer.get());

        auto it = std::remove_if(coins.begin(), coins.end(),
            [&](const SDL_Rect& c) {
                bool hit = SDL_HasIntersection(&player, &c);
                if (hit) score += 10;
                return hit;
            });
        coins.erase(it, coins.end());

        SDL_SetRenderDrawColor(renderer.get(), 0, 120, 255, 255);
        SDL_RenderFillRect(renderer.get(), &player);

        SDL_SetRenderDrawColor(renderer.get(), 255, 215, 0, 255);
        for (const auto& c : coins) {
            SDL_RenderFillRect(renderer.get(), &c);
        }

        if (font) {
            auto surf = TTF_RenderText_Solid(font,
                ("Score: " + std::to_string(score)).c_str(), {255,255,255,255});
            if (surf) {
                auto tex = SDL_CreateTextureFromSurface(renderer.get(), surf);
                SDL_Rect dst = {10, 10, surf->w, surf->h};
                SDL_RenderCopy(renderer.get(), tex, nullptr, &dst);
                SDL_FreeSurface(surf);
                SDL_DestroyTexture(tex);
            }
        }

        if (coins.empty() && font) {
            auto surf = TTF_RenderText_Solid(font, "You Win!", {0,255,0,255});
            if (surf) {
                auto tex = SDL_CreateTextureFromSurface(renderer.get(), surf);
                SDL_Rect dst = {300, 280, surf->w, surf->h};
                SDL_RenderCopy(renderer.get(), tex, nullptr, &dst);
                SDL_FreeSurface(surf);
                SDL_DestroyTexture(tex);
            }
        }

        SDL_RenderPresent(renderer.get());
        SDL_Delay(16);
    }

    TTF_CloseFont(font);
    TTF_Quit();
    SDL_Quit();
    return 0;
}
