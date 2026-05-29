// SDL2 coin-collection game - move a player to collect coins.
#include <SDL2/SDL.h>
#include <SDL2/SDL_ttf.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define WINDOW_W 800
#define WINDOW_H 600
#define PLAYER_SIZE 40
#define COIN_SIZE 20
#define NUM_COINS 10

// Game state: window, renderer, player, coins, score.
typedef struct {
    SDL_Window *window;
    SDL_Renderer *renderer;
    TTF_Font *font;
    SDL_Rect player;
    SDL_Rect coins[NUM_COINS];
    int num_coins;
    int score;
    int running;
} Game;

// Initialize SDL, window, renderer, font, and coin positions.
static int game_init(Game *g) {
    if (SDL_Init(SDL_INIT_VIDEO) < 0) return -1;
    if (TTF_Init() < 0) { SDL_Quit(); return -1; }

    g->window = SDL_CreateWindow("SDL2 Game",
        SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED,
        WINDOW_W, WINDOW_H, SDL_WINDOW_SHOWN);
    if (!g->window) { TTF_Quit(); SDL_Quit(); return -1; }

    g->renderer = SDL_CreateRenderer(g->window, -1, SDL_RENDERER_ACCELERATED);
    if (!g->renderer) { SDL_DestroyWindow(g->window); TTF_Quit(); SDL_Quit(); return -1; }

    g->font = TTF_OpenFont("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf", 24);
    g->player = (SDL_Rect){WINDOW_W/2 - PLAYER_SIZE/2, WINDOW_H/2 - PLAYER_SIZE/2, PLAYER_SIZE, PLAYER_SIZE};
    g->score = 0;
    g->num_coins = NUM_COINS;
    g->running = 1;

    srand((unsigned)time(NULL));
    for (int i = 0; i < NUM_COINS; i++) {
        g->coins[i] = (SDL_Rect){
            rand() % (WINDOW_W - COIN_SIZE),
            rand() % (WINDOW_H - COIN_SIZE),
            COIN_SIZE, COIN_SIZE
        };
    }
    return 0;
}

// Clean up SDL resources.
static void game_cleanup(Game *g) {
    if (g->font) TTF_CloseFont(g->font);
    if (g->renderer) SDL_DestroyRenderer(g->renderer);
    if (g->window) SDL_DestroyWindow(g->window);
    TTF_Quit();
    SDL_Quit();
}

// Update game logic: handle input and collision detection.
static void game_update(Game *g) {
    const Uint8 *keys = SDL_GetKeyboardState(NULL);
    int speed = 5;
    if (keys[SDL_SCANCODE_LEFT]) g->player.x -= speed;
    if (keys[SDL_SCANCODE_RIGHT]) g->player.x += speed;
    if (keys[SDL_SCANCODE_UP]) g->player.y -= speed;
    if (keys[SDL_SCANCODE_DOWN]) g->player.y += speed;

    for (int i = 0; i < g->num_coins; i++) {
        if (SDL_HasIntersection(&g->player, &g->coins[i])) {
            g->coins[i] = g->coins[--g->num_coins];
            g->score += 10;
            i--;
        }
    }
}

// Render the game: clear screen, draw player and coins, show UI.
static void game_render(Game *g) {
    SDL_SetRenderDrawColor(g->renderer, 30, 30, 30, 255);
    SDL_RenderClear(g->renderer);

    SDL_SetRenderDrawColor(g->renderer, 0, 120, 255, 255);
    SDL_RenderFillRect(g->renderer, &g->player);

    SDL_SetRenderDrawColor(g->renderer, 255, 215, 0, 255);
    for (int i = 0; i < g->num_coins; i++)
        SDL_RenderFillRect(g->renderer, &g->coins[i]);

    if (g->font) {
        char buf[64];
        snprintf(buf, sizeof(buf), "Score: %d", g->score);
        SDL_Surface *surf = TTF_RenderText_Solid(g->font, buf, (SDL_Color){255,255,255,255});
        if (surf) {
            SDL_Texture *tex = SDL_CreateTextureFromSurface(g->renderer, surf);
            SDL_Rect dst = {10, 10, surf->w, surf->h};
            SDL_RenderCopy(g->renderer, tex, NULL, &dst);
            SDL_FreeSurface(surf);
            SDL_DestroyTexture(tex);
        }
    }

    if (g->num_coins == 0 && g->font) {
        SDL_Surface *surf = TTF_RenderText_Solid(g->font, "You Win!", (SDL_Color){0,255,0,255});
        if (surf) {
            SDL_Texture *tex = SDL_CreateTextureFromSurface(g->renderer, surf);
            SDL_Rect dst = {WINDOW_W/2 - surf->w/2, WINDOW_H/2 - surf->h/2, surf->w, surf->h};
            SDL_RenderCopy(g->renderer, tex, NULL, &dst);
            SDL_FreeSurface(surf);
            SDL_DestroyTexture(tex);
        }
    }

    SDL_RenderPresent(g->renderer);
}

// Entry point: init game, run game loop, cleanup.
int main(int argc, char *argv[]) {
    Game game;
    if (game_init(&game) != 0) {
        fprintf(stderr, "Failed to initialize game\n");
        return 1;
    }

    while (game.running) {
        SDL_Event e;
        while (SDL_PollEvent(&e)) {
            if (e.type == SDL_QUIT) game.running = 0;
        }
        game_update(&game);
        game_render(&game);
        SDL_Delay(16);
    }

    game_cleanup(&game);
    return 0;
}
