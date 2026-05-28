/// @description Runner player for endless runner

// Create event
x = 128;
y = 256;
vspeed = 0;
gravity = 0.8;
jump_power = -14;
is_grounded = false;
is_sliding = false;
slide_timer = 0;
score = 0;
highscore = 0;
game_over = false;
speed_mult = 1.0;
invincible = false;

// Load highscore
var _ini = ini_open("runner_data.ini");
highscore = ini_read_real("Score", "Highscore", 0);
ini_close();

// Step event
if (game_over) exit;

// Ground check
is_grounded = place_meeting(x, y + 1, obj_ground);

// Jump
if (keyboard_check_pressed(vk_space) && is_grounded && !is_sliding) {
    vspeed = jump_power;
    audio_play_sound(snd_jump, 1, false);
}

// Slide
if (keyboard_check_pressed(vk_down) && is_grounded && !is_sliding) {
    is_sliding = true;
    slide_timer = 30;
    mask_index = spr_player_slide;
} else if (is_sliding) {
    slide_timer--;
    if (slide_timer <= 0) {
        is_sliding = false;
        mask_index = spr_player;
    }
}

// Gravity
vspeed += gravity;
y += vspeed;

// Collision with ground
if (y > room_height - 64) {
    y = room_height - 64;
    vspeed = 0;
}

// Score increase over time
score += 0.1 * speed_mult;

// Speed increase over time
speed_mult = 1.0 + floor(score / 100) * 0.1;
global.scroll_speed = 4 * speed_mult;

// Collision with obstacles
if (!invincible && place_meeting(x, y, obj_obstacle)) {
    if (is_sliding && other.obstacle_type == "high") {
        // Slide under high obstacles
    } else {
        game_over = true;
        // Save highscore
        if (score > highscore) {
            highscore = score;
            var _ini = ini_open("runner_data.ini");
            ini_write_real("Score", "Highscore", highscore);
            ini_close();
        }
        audio_play_sound(snd_death, 1, false);
    }
}

// Draw GUI event
draw_set_color(c_white);
draw_set_halign(fa_left);
draw_text(16, 16, "Score: " + string(floor(score)));
draw_text(16, 40, "Highscore: " + string(floor(highscore)));

if (game_over) {
    draw_set_halign(fa_center);
    draw_set_color(c_red);
    draw_text(room_width / 2, room_height / 2 - 20, "GAME OVER");
    draw_set_color(c_white);
    draw_text(room_width / 2, room_height / 2 + 10, "Press SPACE to restart");
    if (keyboard_check_pressed(vk_space)) {
        game_restart();
    }
}
