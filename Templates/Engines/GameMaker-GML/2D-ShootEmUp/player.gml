/// @description Player ship for shoot-em-up

// Create event
hp = 3;
max_hp = 3;
move_speed = 5;
fire_rate = 8;
fire_cooldown = 0;
power_level = 1;
score = 0;
lives = 3;
invincible = 0;

// Step event
// Mouse movement
var _mx = device_mouse_x_to_gui(0);
var _my = device_mouse_y_to_gui(0);
x = lerp(x, _mx, 0.15);
y = lerp(y, _my, 0.15);

// Clamp to room
x = clamp(x, 32, room_width - 32);
y = clamp(y, 32, room_height - 32);

// Shooting
if (fire_cooldown > 0) {
    fire_cooldown--;
}
var _shoot = mouse_check_button(mb_left) || keyboard_check(vk_space);
if (_shoot && fire_cooldown == 0) {
    fire_cooldown = fire_rate;
    shoot_bullets();
}

// Invincibility
if (invincible > 0) {
    invincible--;
    image_alpha = (invincible mod 6 < 3) ? 0.5 : 1.0;
} else {
    image_alpha = 1.0;
}

function shoot_bullets() {
    var _bullet = obj_bullet;
    switch (power_level) {
        case 1:
            instance_create_layer(x, y - 16, "Bullets", _bullet);
            break;
        case 2:
            instance_create_layer(x - 8, y - 12, "Bullets", _bullet);
            instance_create_layer(x + 8, y - 12, "Bullets", _bullet);
            break;
        case 3:
            instance_create_layer(x, y - 20, "Bullets", _bullet);
            instance_create_layer(x - 16, y - 8, "Bullets", _bullet);
            instance_create_layer(x + 16, y - 8, "Bullets", _bullet);
            break;
    }
    audio_play_sound(snd_shoot, 1, false);
}

// Collision with enemy bullet
function hit() {
    if (invincible > 0) return;
    hp--;
    invincible = 60;
    if (hp <= 0) {
        lives--;
        if (lives > 0) {
            hp = max_hp;
            x = room_width / 2;
            y = room_height - 64;
        } else {
            game_restart();
        }
    }
}
