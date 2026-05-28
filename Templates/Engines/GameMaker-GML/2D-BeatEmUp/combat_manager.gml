/// @description Combat manager for beat 'em up

// Create event
score = 0;
wave = 0;
enemies_alive = 0;
max_enemies = 5;
spawn_timer = 0;

// Step event
enemies_alive = instance_number(obj_enemy);

if (enemies_alive < max_enemies && spawn_timer <= 0) {
    // Spawn enemy at random position off-screen
    var _side = irandom(3);
    var _sx = 0;
    var _sy = irandom_range(64, room_height - 64);
    switch (_side) {
        case 0: _sx = -32; break;
        case 1: _sx = room_width + 32; break;
        case 2: _sx = irandom_range(0, room_width); _sy = -32; break;
        case 3: _sx = irandom_range(0, room_width); _sy = room_height + 32; break;
    }
    var _enemy = instance_create_layer(_sx, _sy, "Enemies", obj_enemy_grunt);
    spawn_timer = 60;
} else if (spawn_timer > 0) {
    spawn_timer--;
}

// Wave system
var _total = instance_number(obj_enemy_grunt) + instance_number(obj_enemy_brute);
if (_total == 0 && spawn_timer <= -60) {
    wave++;
    max_enemies = 3 + wave * 2;
    spawn_timer = -60;
    // Spawn a brute every 3 waves
    if (wave mod 3 == 0) {
        instance_create_layer(room_width / 2, -32, "Enemies", obj_enemy_brute);
    }
}

// Draw GUI
draw_set_color(c_white);
draw_text(16, 16, "Score: " + string(score));
draw_text(16, 40, "Wave: " + string(wave));
draw_text(16, 64, "HP: " + string(obj_player.hp) + "/" + string(obj_player.max_hp));
