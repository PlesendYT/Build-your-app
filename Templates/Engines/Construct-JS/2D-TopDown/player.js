// Player Health & Combat - Construct 3 Script

let playerHP = 100;
const maxHP = 100;

export function setupPlayerStats() {
    playerHP = maxHP;
}

export function takeDamage(amount) {
    playerHP = Math.max(0, playerHP - amount);
    runtime.callFunction("UpdateHealthBar", playerHP, maxHP);

    if (playerHP <= 0) {
        runtime.callFunction("DisplayMessage", "Game Over!");
        runtime.callFunction("RestartLayout");
    }
}

export function healPlayer(amount) {
    playerHP = Math.min(maxHP, playerHP + amount);
    runtime.callFunction("UpdateHealthBar", playerHP, maxHP);
}

export function getPlayerHP() {
    return playerHP;
}

export function getMaxHP() {
    return maxHP;
}

// Inventory system
let inventory = [];

export function addToInventory(itemName) {
    inventory.push(itemName);
    runtime.callFunction("UpdateInventory", inventory.join(", "));
}

export function removeFromInventory(itemName) {
    const idx = inventory.indexOf(itemName);
    if (idx !== -1) {
        inventory.splice(idx, 1);
        return true;
    }
    return false;
}

export function hasItem(itemName) {
    return inventory.includes(itemName);
}

export function getInventory() {
    return [...inventory];
}

// Register functions
globalThis.TakeDamage = takeDamage;
globalThis.HealPlayer = healPlayer;
globalThis.AddToInventory = addToInventory;
