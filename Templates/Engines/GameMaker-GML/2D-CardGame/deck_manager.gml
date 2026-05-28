/// @description Deck manager for card game

// Create event
deck = [];
hand = [];
discard = [];
exhaust = [];
energy = 3;
max_energy = 3;
player_hp = 30;
player_max_hp = 30;
player_block = 0;
enemy_hp = 40;
enemy_max_hp = 40;
enemy_block = 0;
enemy_intent = "";

turn = 0;
phase = "player"; // "player", "enemy", "gameover"

// Initialize deck
deck = create_default_deck();
shuffle_deck();
draw_hand(5);

function shuffle_deck() {
    // Fisher-Yates shuffle
    for (var i = array_length(deck) - 1; i > 0; i--) {
        var _j = irandom(i);
        var _tmp = deck[i];
        deck[i] = deck[j];
        deck[j] = _tmp;
    }
}

function draw_card() {
    if (array_length(deck) == 0) {
        // Reshuffle discard pile
        deck = discard;
        discard = [];
        shuffle_deck();
    }
    if (array_length(deck) > 0) {
        var _card = array_pop(deck);
        array_push(hand, _card);
        return _card;
    }
    return undefined;
}

function draw_hand(_count) {
    for (var i = 0; i < _count; i++) {
        draw_card();
    }
}

function play_card(_card_id, _target) {
    var _idx = -1;
    for (var i = 0; i < array_length(hand); i++) {
        if (hand[i].id == _card_id) {
            _idx = i;
            break;
        }
    }
    if (_idx == -1 || hand[_idx].cost > energy) return false;

    var _card = hand[_idx];
    energy -= _card.cost;

    switch (_card.type) {
        case "attack":
            var _dmg = _card.value;
            if (enemy_block > 0) {
                if (_dmg <= enemy_block) {
                    enemy_block -= _dmg;
                    _dmg = 0;
                } else {
                    _dmg -= enemy_block;
                    enemy_block = 0;
                }
            }
            enemy_hp -= _dmg;
            break;
        case "defense":
            player_block += _card.value;
            break;
        case "spell":
            player_hp = min(player_max_hp, player_hp + _card.value);
            break;
    }

    array_delete(hand, _idx, 1);
    array_push(discard, _card);
    return true;
}

function end_player_turn() {
    // Discard hand
    for (var i = 0; i < array_length(hand); i++) {
        array_push(discard, hand[i]);
    }
    hand = [];
    energy = max_energy;
    player_block = 0;
    phase = "enemy";
}

function enemy_turn() {
    // Simple enemy AI
    var _dmg = irandom_range(4, 8);
    if (player_block > 0) {
        if (_dmg <= player_block) {
            player_block -= _dmg;
        } else {
            _dmg -= player_block;
            player_block = 0;
            player_hp -= _dmg;
        }
    } else {
        player_hp -= _dmg;
    }
    enemy_block = 0;

    if (player_hp <= 0) {
        phase = "gameover";
        return;
    }

    phase = "player";
    turn++;
    draw_hand(5);
}
