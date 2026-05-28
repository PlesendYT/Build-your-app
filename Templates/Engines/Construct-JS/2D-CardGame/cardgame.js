// Card Game Core - Construct 3 Script

const SUITS = ["hearts", "diamonds", "clubs", "spades"];
const RANKS = ["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"];

let playerDeck = [];
let playerHand = [];
let enemyDeck = [];
let enemyHand = [];
let discardPile = [];
let playerHP = 30;
let enemyHP = 30;
let playerMana = 1;
let maxMana = 1;
let turn = 1;
let phase = "player_draw";
let gameOver = false;

export function initCardGame() {
    playerDeck = createDeck();
    enemyDeck = createDeck();
    playerHand = [];
    enemyHand = [];
    discardPile = [];
    playerHP = 30;
    enemyHP = 30;
    playerMana = 1;
    maxMana = 1;
    turn = 1;
    phase = "player_draw";
    gameOver = false;

    shuffleDeck(playerDeck);
    shuffleDeck(enemyDeck);

    // Draw starting hands
    for (let i = 0; i < 3; i++) {
        playerHand.push(playerDeck.pop());
        enemyHand.push(enemyDeck.pop());
    }
}

function createDeck() {
    const deck = [];
    for (const suit of SUITS) {
        for (const rank of RANKS) {
            const value = getCardValue(rank);
            deck.push({ suit, rank, value });
        }
    }
    return deck;
}

function getCardValue(rank) {
    if (rank === "A") return 14;
    if (rank === "K") return 13;
    if (rank === "Q") return 12;
    if (rank === "J") return 11;
    return parseInt(rank);
}

function shuffleDeck(deck) {
    for (let i = deck.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [deck[i], deck[j]] = [deck[j], deck[i]];
    }
}

export function startPlayerTurn() {
    maxMana = Math.min(10, maxMana + 1);
    playerMana = maxMana;

    // Draw a card
    if (playerDeck.length > 0 || discardPile.length > 0) {
        if (playerDeck.length === 0) {
            // Reshuffle discard
            playerDeck = [...discardPile];
            discardPile = [];
            shuffleDeck(playerDeck);
        }
        playerHand.push(playerDeck.pop());
    }
    phase = "player_action";
}

export function playCard(cardIndex) {
    if (phase !== "player_action" || cardIndex >= playerHand.length) return false;

    const card = playerHand[cardIndex];
    const manaCost = Math.ceil(card.value / 3);

    if (playerMana < manaCost) return false;

    playerMana -= manaCost;

    // Apply card effect
    const damage = card.value;
    enemyHP -= damage;
    discardPile.push(card);
    playerHand.splice(cardIndex, 1);

    if (enemyHP <= 0) {
        gameOver = true;
        runtime.callFunction("DisplayMessage", "You win!");
    }

    return true;
}

export function endPlayerTurn() {
    // Discard hand
    while (playerHand.length > 0) {
        discardPile.push(playerHand.pop());
    }
    phase = "enemy_turn";
    executeEnemyTurn();
}

function executeEnemyTurn() {
    // Enemy AI: play cards randomly
    const playableCards = enemyHand.filter(c => c.value <= 10);
    for (const card of playableCards) {
        playerHP -= Math.floor(card.value / 2);
    }

    // Enemy draws
    if (enemyDeck.length > 0) {
        enemyHand.push(enemyDeck.pop());
    }
    enemyHand.push(enemyDeck.pop());

    turn++;
    if (playerHP <= 0) {
        gameOver = true;
        runtime.callFunction("DisplayMessage", "Game Over!");
    }

    phase = "player_draw";
}

export function getCardDisplay(card) {
    return `${card.rank} of ${card.suit} (${card.value})`;
}

export function getGameState() {
    return {
        playerHP,
        enemyHP,
        playerMana,
        maxMana,
        turn,
        phase,
        playerHand: playerHand.map(getCardDisplay),
        enemyHandCount: enemyHand.length,
        playerDeckCount: playerDeck.length,
        gameOver
    };
}

// Card animation helper
export function animateCardPlay(fromX, fromY, toX, toY, cardData) {
    // Simplified animation using timer
    const cardDisplay = document.createElement("div");
    cardDisplay.textContent = getCardDisplay(cardData);
}
