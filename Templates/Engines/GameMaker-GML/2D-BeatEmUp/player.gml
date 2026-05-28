/// @description Player for beat 'em up

// Create event
hp = 10;
max_hp = 10;
move_speed = 4;
combo_count = 0;
combo_timer = 0;
max_combo_time = 20;
is_attacking = false;
attack_frame = 0;
special_cooldown = 0;
facing = 1; // 1=right, -1=left
state = "idle"; // "idle", "walk", "attack", "hit", "special"
invincible = 0;

// Step event
if (state == "hit" || state == "special") {
    // Wait for animation to finish
    if (invincible > 0) invincible--;
    exit;
}

var _left = keyboard_check(vk_left);
var _right = keyboard_check(vk_right);
var _up = keyboard_check(vk_up);
var _down = keyboard_check(vk_down);
var _attack = keyboard_check_pressed(vk_space);
var _special = keyboard_check_pressed(ord("S"));

// Movement
if (!is_attacking) {
    var _move = _right - _left;
    if (_move != 0) {
        facing = _move;
        x += _move * move_speed;
        state = "walk";
    } else {
        state = "idle";
    }
    image_xscale = facing;
}

// Attack combo
if (_attack && !is_attacking && state != "hit") {
    if (combo_timer > 0 && combo_count < 3) {
        combo_count++;
    } else {
        combo_count = 1;
    }
    combo_timer = max_combo_time;
    is_attacking = true;
    state = "attack";
    attack_frame = 0;

    // Create hitbox
    var _hb = instance_create_layer(x + facing * 32, y, "Hitboxes", obj_hitbox);
    with (_hb) {
        damage = 2 + other.combo_count;
        knockback = 4 + other.combo_count * 2;
        dir = other.facing;
        alarm[0] = 8;
    }
}

// Special attack
if (_special && special_cooldown == 0 && !is_attacking) {
    state = "special";
    special_cooldown = 60;
    // AoE attack
    var _hb = instance_create_layer(x, y, "Hitboxes", obj_hitbox);
    with (_hb) {
        image_xscale = 3;
        damage = 8;
        knockback = 10;
        is_aoe = true;
        alarm[0] = 12;
    }
}

if (combo_timer > 0) {
    combo_timer--;
    if (combo_timer == 0 && is_attacking) {
        is_attacking = false;
        state = "idle";
        combo_count = 0;
    }
}

if (special_cooldown > 0) special_cooldown--;

// Collision with enemy attacks
if (invincible > 0) {
    invincible--;
}
