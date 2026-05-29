// Number guessing game - player guesses a number between 1-100 in 7 tries.
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define MAX_ATTEMPTS 7
#define LOWER 1
#define UPPER 100

int main(void) {
    // Seed RNG and generate the secret number.
    srand((unsigned)time(NULL));
    int secret = rand() % (UPPER - LOWER + 1) + LOWER;
    int attempts = 0;
    int guess;

    printf("Guess a number between %d and %d!\n", LOWER, UPPER);
    printf("You have %d attempts.\n", MAX_ATTEMPTS);

    // Main game loop: prompt, read guess, give feedback.
    while (attempts < MAX_ATTEMPTS) {
        printf("Attempt %d: ", attempts + 1);
        if (scanf("%d", &guess) != 1) {
            while (getchar() != '\n');
            printf("Invalid input.\n");
            continue;
        }

        attempts++;

        if (guess < secret) {
            printf("Too low!\n");
        } else if (guess > secret) {
            printf("Too high!\n");
        } else {
            printf("Correct! You got it in %d attempts!\n", attempts);
            return 0;
        }
    }

    printf("Game over! The number was %d.\n", secret);
    return 0;
}
