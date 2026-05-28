// Deck Manager - Construct 3 Script

// Special card definitions for Construct card game
const specialCards = [
    { name: "Fireball", type: "spell", value: 8, manaCost: 3, effect: "damage_all" },
    { name: "Heal", type: "spell", value: 6, manaCost: 2, effect: "heal_self" },
    { name: "Shield", type: "spell", value: 4, manaCost: 2, effect: "block" },
    { name: "Lightning", type: "spell", value: 12, manaCost: 5, effect: "damage_single" },
    { name: "Poison", type: "spell", value: 3, manaCost: 2, effect: "damage_over_time" },
    { name: "Sword", type: "attack", value: 5, manaCost: 1, effect: "damage_single" },
    { name: "Axe", type: "attack", value: 7, manaCost: 2, effect: "damage_single" },
    { name: "Bow", type: "attack", value: 4, manaCost: 1, effect: "damage_any" },
    { name: "Dagger", type: "attack", value: 3, manaCost: 1, effect: "damage_twice" },
    { name: "Staff", type: "attack", value: 6, manaCost: 2, effect: "damage_magic" },
];

export function createSpecialDeck(count = 10) {
    const deck = [];
    // Add special cards
    for (let i = 0; i < count && i < specialCards.length; i++) {
        deck.push({ ...specialCards[i], id: `special_${i}` });
    }
    // Fill with standard cards
    while (deck.length < 20) {
        const rank = RANKS[Math.floor(Math.random() * RANKS.length)];
        const suit = SUITS[Math.floor(Math.random() * SUITS.length)];
        deck.push({
            name: `${rank} of ${suit}`,
            type: "standard",
            value: getCardValue(rank),
            manaCost: Math.ceil(getCardValue(rank) / 3),
            suit,
            rank
        });
    }
    return deck;
}

export function getCardDescription(card) {
    switch (card.effect) {
        case "damage_all": return `Deal ${card.value} damage to all enemies`;
        case "heal_self": return `Heal ${card.value} HP`;
        case "block": return `Gain ${card.value} block`;
        case "damage_single": return `Deal ${card.value} damage`;
        case "damage_over_time": return `Deal ${card.value} damage each turn for 3 turns`;
        case "damage_twice": return `Deal ${Math.floor(card.value / 2)} damage twice`;
        case "damage_magic": return `Deal ${card.value} magic damage (ignores armor)`;
        case "damage_any": return `Deal ${card.value} damage to any target`;
        default: return `Value: ${card.value}`;
    }
}

export function applySpecialEffect(card, target, source) {
    switch (card.effect) {
        case "damage_all":
            return { damage: card.value, type: "aoe" };
        case "heal_self":
            return { heal: card.value };
        case "block":
            return { block: card.value };
        case "damage_single":
        case "damage_any":
            return { damage: card.value, type: "single" };
        case "damage_twice":
            return { damage: Math.floor(card.value / 2), type: "double" };
        case "damage_magic":
            return { damage: card.value, type: "magic" };
        default:
            return { damage: card.value, type: "single" };
    }
}
