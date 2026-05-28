#include <SDL2/SDL.h>
#include <SDL2/SDL_ttf.h>
#include <iostream>
#include <memory>
#include <stdexcept>
#include <string>

struct SDL_Deleter {
    void operator()(SDL_Window* w) const { SDL_DestroyWindow(w); }
    void operator()(SDL_Renderer* r) const { SDL_DestroyRenderer(r); }
};

using WindowPtr = std::unique_ptr<SDL_Window, SDL_Deleter>;
using RendererPtr = std::unique_ptr<SDL_Renderer, SDL_Deleter>;

class SDLApp {
public:
    SDLApp(const char* title, int w, int h)
        : width_{w}, height_{h} {
        if (SDL_Init(SDL_INIT_VIDEO) < 0) {
            throw std::runtime_error{SDL_GetError()};
        }
        if (TTF_Init() < 0) {
            SDL_Quit();
            throw std::runtime_error{TTF_GetError()};
        }
        window_.reset(SDL_CreateWindow(title, SDL_WINDOWPOS_CENTERED,
            SDL_WINDOWPOS_CENTERED, w, h, SDL_WINDOW_SHOWN));
        if (!window_) {
            TTF_Quit();
            SDL_Quit();
            throw std::runtime_error{SDL_GetError()};
        }
        renderer_.reset(SDL_CreateRenderer(window_.get(), -1,
            SDL_RENDERER_ACCELERATED));
        if (!renderer_) {
            TTF_Quit();
            SDL_Quit();
            throw std::runtime_error{SDL_GetError()};
        }
        font_ = TTF_OpenFont("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf", 24);
    }

    ~SDLApp() {
        if (font_) TTF_CloseFont(font_);
        TTF_Quit();
        SDL_Quit();
    }

    auto run() -> void {
        bool running = true;
        while (running) {
            SDL_Event event;
            while (SDL_PollEvent(&event)) {
                if (event.type == SDL_QUIT) running = false;
            }

            SDL_SetRenderDrawColor(renderer_.get(), 30, 30, 30, 255);
            SDL_RenderClear(renderer_.get());

            SDL_SetRenderDrawColor(renderer_.get(), 0, 120, 255, 255);
            SDL_Rect rect{100, 100, 200, 200};
            SDL_RenderFillRect(renderer_.get(), &rect);

            if (font_) {
                auto* surface = TTF_RenderText_Solid(font_,
                    "Hello, SDL2!", {255, 255, 255, 255});
                if (surface) {
                    auto* texture = SDL_CreateTextureFromSurface(
                        renderer_.get(), surface);
                    SDL_Rect dst{300, 280, surface->w, surface->h};
                    SDL_RenderCopy(renderer_.get(), texture, nullptr, &dst);
                    SDL_FreeSurface(surface);
                    SDL_DestroyTexture(texture);
                }
            }

            SDL_RenderPresent(renderer_.get());
        }
    }

private:
    int width_, height_;
    WindowPtr window_;
    RendererPtr renderer_;
    TTF_Font* font_ = nullptr;
};

auto main() -> int {
    try {
        SDLApp app{"SDL2 Template", 800, 600};
        app.run();
    } catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << "\n";
        return 1;
    }
    return 0;
}
