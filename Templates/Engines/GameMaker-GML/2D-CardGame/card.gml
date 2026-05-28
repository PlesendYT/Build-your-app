/// @description Card struct and functions

// Card struct constructor
function Card(_name, _cost, _type, _value, _description) constructor {
    name = _name;
    cost = _cost;
    type = _type; // "attack", "defense", "spell"
    value = _value;
    description = _description;
    id = "";
}

// Create default cards
function create_default_deck() {
    var _deck = [];

    array_push(_deck, new Card("Strike", 1, "attack", 6, "Deal 6 damage"));
    array_push(_deck, new Card("Strike", 1, "attack", 6, "Deal 6 damage"));
    array_push(_deck, new Card("Strike", 1, "attack", 6, "Deal 6 damage"));
    array_push(_deck, new Card("Defend", 1, "defense", 5, "Gain 5 block"));
    array_push(_deck, new Card("Defend", 1, "defense", 5, "Gain 5 block"));
    array_push(_deck, new Card("Defend", 1, "defense", 5, "Gain 5 block"));
    array_push(_deck, new Card("Fireball", 2, "attack", 12, "Deal 12 damage"));
    array_push(_deck, new Card("Heal", 2, "spell", 8, "Heal 8 HP"));
    array_push(_deck, new Card("Shield Bash", 2, "attack", 4, "Deal 4 damage, gain 4 block"));
    array_push(_deck, new Card("Lightning", 3, "attack", 18, "Deal 18 damage"));

    // Assign IDs
    for (var i = 0; i < array_length(_deck); i++) {
        _deck[i].id = "card_" + string(i);
    }
    return _deck;
}
