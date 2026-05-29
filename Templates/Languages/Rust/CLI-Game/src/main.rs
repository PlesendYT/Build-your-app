//! A number-guessing game played in the terminal.
//! The player has a limited number of attempts to guess the secret number.

use anyhow::Result;
use rand::Rng;
use std::io::{self, Write};

/// Tracks the secret number, remaining attempts, and the valid range.
struct Game {
    secret: u32,
    attempts: u32,
    max_attempts: u32,
    lower: u32,
    upper: u32,
}

impl Game {
    /// Creates a new game with a random secret in `[lower, upper]`.
    fn new(lower: u32, upper: u32, max_attempts: u32) -> Self {
        let secret = rand::rng().random_range(lower..=upper);
        Self { secret, attempts: 0, max_attempts, lower, upper }
    }

    /// Processes a guess. Returns `Some(hint)` if wrong, `None` if correct.
    fn guess(&mut self, n: u32) -> Option<&'static str> {
        self.attempts += 1;
        if n < self.secret {
            Some("zu niedrig / too low")
        } else if n > self.secret {
            Some("zu hoch / too high")
        } else {
            None
        }
    }

    /// Returns `true` if the player has used all their attempts.
    fn is_over(&self) -> bool {
        self.attempts >= self.max_attempts
    }

    /// Runs the main game loop: prompt, read, validate, check guess.
    fn play(&mut self) -> Result<()> {
        println!("Rate eine Zahl zwischen {} und {}!", self.lower, self.upper);
        println!("Du hast {} Versuche.", self.max_attempts);

        loop {
            print!("Versuch {}: ", self.attempts + 1);
            io::stdout().flush()?;
            let mut input = String::new();
            io::stdin().read_line(&mut input)?;

            let n: u32 = match input.trim().parse() {
                Ok(n) if n >= self.lower && n <= self.upper => n,
                _ => {
                    println!("Bitte eine Zahl zwischen {} und {}.", self.lower, self.upper);
                    continue;
                }
            };

            match self.guess(n) {
                Some(hint) => println!("{hint}"),
                None => {
                    println!("Richtig! Du hast es in {} Versuchen geschafft!", self.attempts);
                    return Ok(());
                }
            }

            if self.is_over() {
                println!("Leider verloren. Die Zahl war {}.", self.secret);
                return Ok(());
            }
        }
    }
}

/// Entry point: starts a new game with default parameters.
fn main() -> Result<()> {
    let mut game = Game::new(1, 100, 7);
    game.play()
}
