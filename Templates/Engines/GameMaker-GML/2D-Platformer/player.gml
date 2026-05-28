/// @description Player controller for 2D platformer

// Create event
x = 64;
y = 64;
hp = 3;
max_hp = 3;
score = 0;
has_double_jump = false;
can_double_jump = false;
move_speed = 4;
jump_power = -12;
gravity = 0.6;
friction = 0.2;

// Step event
var _left = keyboard_check(vk_left);
var _right = keyboard_check(vk_right);
var _jump = keyboard_check_pressed(vk_space);
var _on_ground = place_meeting(x, y + 1, obj_ground);

// Horizontal movement
if (_left) {
    hspeed = max(hspeed - 1, -move_speed);
    image_xscale = -1;
} else if (_right) {
    hspeed = min(hspeed + 1, move_speed);
    image_xscale = 1;
} else {
    hspeed = lerp(hspeed, 0, friction);
}

// Jump
if (_on_ground) {
    can_double_jump = true;
    if (_jump) {
        vspeed = jump_power;
    }
} else {
    if (_jump && can_double_jump && has_double_jump) {
        vspeed = jump_power * 0.8;
        can_double_jump = false;
    }
}

// Gravity
vspeed += gravity;

// Collision with enemies
if (place_meeting(x, y, obj_enemy)) {
    if (vspeed > 0 && y < other.y - 16) {
        vspeed = jump_power * 0.5;
        with (other) { instance_destroy(); }
    } else {
        hp -= 1;
        vspeed = jump_power * 0.3;
        if (hp <= 0) {
            game_restart();
        }
    }
}

// Collect coins
if (place_meeting(x, y, obj_coin)) {
    with (other) {
        instance_destroy();
        other_do = function() {
            score += 10;
        }
    }
    // Audio: audio_play_sound(snd_coin, 1, false);
}
