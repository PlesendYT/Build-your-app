/// @description Player controller for top-down game

// Create event
hp = 5;
max_hp = 5;
move_speed = 3.5;
inventory = [];
max_inventory = 10;
keys = 0;
attack_cooldown = 0;
attack_rate = 15;

// Step event
var _move_x = keyboard_check(ord("D")) - keyboard_check(ord("A"));
var _move_y = keyboard_check(ord("S")) - keyboard_check(ord("W"));
var _attack = keyboard_check_pressed(vk_space);

// Normalize diagonal movement
if (_move_x != 0 && _move_y != 0) {
    _move_x *= 0.707;
    _move_y *= 0.707;
}

// Move with collision
var _new_x = x + _move_x * move_speed;
var _new_y = y + _move_y * move_speed;

if (!place_meeting(_new_x, y, obj_wall)) {
    x = _new_x;
}
if (!place_meeting(x, _new_y, obj_wall)) {
    y = _new_y;
}

// Sprite direction
if (_move_x != 0 || _move_y != 0) {
    image_index = 0;
    image_speed = 0.3;
    if (abs(_move_x) > abs(_move_y)) {
        if (_move_x > 0) sprite_index = spr_player_right;
        else sprite_index = spr_player_left;
    } else {
        if (_move_y > 0) sprite_index = spr_player_down;
        else sprite_index = spr_player_up;
    }
} else {
    image_speed = 0;
}

// Attack
if (attack_cooldown > 0) {
    attack_cooldown--;
}
if (_attack && attack_cooldown == 0) {
    attack_cooldown = attack_rate;
    var _inst = instance_create_layer(x, y, "Instances", obj_attack_hitbox);
    with (_inst) {
        image_xscale = other.image_xscale;
        // Direction based on facing
    }
}

// Pick up items
var _item = instance_place(x, y, obj_item);
if (_item != noone && array_length(inventory) < max_inventory) {
    array_push(inventory, _item.item_name);
    with (_item) { instance_destroy(); }
}
