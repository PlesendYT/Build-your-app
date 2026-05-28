/// @description Fighter for fighting game

// Create event
player_id = 1; // 1 or 2
hp = 100;
max_hp = 100;
energy = 0;
max_energy = 100;
facing = 1;
move_speed = 5;
jump_power = -15;
gravity = 0.8;
is_grounded = true;
state = "idle";
state_timer = 0;
hitstun = 0;
blockstun = 0;
is_blocking = false;
combo_damage = 0;
input_buffer = [];
special_move_cooldown = 0;

// Input mappings
if (player_id == 1) {
    key_left = vk_left; key_right = vk_right;
    key_up = vk_up; key_down = vk_down;
    key_punch = ord("Z"); key_kick = ord("X");
    key_special = ord("C"); key_block = ord("A");
} else {
    key_left = ord("A"); key_right = ord("D");
    key_up = ord("W"); key_down = ord("S");
    key_punch = ord("K"); key_kick = ord("L");
    key_special = ord("J"); key_block = ord("Q");
}

// Step event
if (hitstun > 0) {
    hitstun--;
    state = "hit";
    exit;
}
if (blockstun > 0) {
    blockstun--;
    state = "block";
    exit;
}

if (special_move_cooldown > 0) special_move_cooldown--;

// Input
var _left = keyboard_check(key_left);
var _right = keyboard_check(key_right);
var _down = keyboard_check(key_down);
var _attack_p = keyboard_check_pressed(key_punch);
var _attack_k = keyboard_check_pressed(key_kick);
var _attack_s = keyboard_check_pressed(key_special);
is_blocking = keyboard_check(key_block);

// Movement
if (!_left && !_right) {
    if (state == "walk") state = "idle";
} else if (_left) {
    if (!is_blocking) {
        x -= move_speed;
        facing = -1;
        state = "walk";
    }
} else if (_right) {
    if (!is_blocking) {
        x += move_speed;
        facing = 1;
        state = "walk";
    }
}

// Clamp to arena
x = clamp(x, 32, room_width - 32);

// Gravity
if (!is_grounded) {
    vspeed += gravity;
    y += vspeed;
    if (y >= room_height - 96) {
        y = room_height - 96;
        vspeed = 0;
        is_grounded = true;
    }
}

// Attacks
if (_attack_p) perform_attack("punch", 8, 5);
if (_attack_k) perform_attack("kick", 12, 8);
if (_attack_s && energy >= 30) {
    energy -= 30;
    perform_attack("special", 25, 15);
}

// Energy gain
energy = min(max_energy, energy + 0.5);

// Face opponent
var _opponent = obj_fighter_manager.opponent(id);
if (_opponent != noone) {
    facing = sign(_opponent.x - x);
}

function perform_attack(_type, _dmg, _knockback) {
    state = _type;
    state_timer = 15;
    image_index = 0;
    image_speed = 0.5;

    var _opponent = obj_fighter_manager.opponent(id);
    if (_opponent != noone) {
        var _dist = abs(x - _opponent.x);
        if (_dist < 64) {
            if (_opponent.is_blocking) {
                _opponent.blockstun = 10;
                _opponent.energy += 2;
                // Push back
                _opponent.x += facing * -8;
            } else {
                _dmg = apply_combo_damage(_dmg);
                with (_opponent) {
                    hp -= _dmg;
                    hitstun = 15;
                    x += other.facing * _knockback;
                }
                combo_damage += _dmg;
            }
        }
    }
}

function apply_combo_damage(_dmg) {
    // Damage scaling for combos
    var _scale = max(0.3, 1.0 - combo_damage * 0.02);
    return ceil(_dmg * _scale);
}
