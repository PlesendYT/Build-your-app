package com.example.minigame;

/**
 * Represents the current state of the minigame.
 * States transition from LOBBY -> COUNTDOWN -> ACTIVE -> ENDED -> LOBBY.
 */
public enum GameState {

    /** Players are waiting in the lobby before the game starts. */
    LOBBY,
    /** Countdown before the game begins. */
    COUNTDOWN,
    /** The game is currently active and players are playing. */
    ACTIVE,
    /** The game has ended and results are shown. */
    ENDED
}
