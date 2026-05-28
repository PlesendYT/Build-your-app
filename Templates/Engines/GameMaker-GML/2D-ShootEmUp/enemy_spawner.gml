/// @description Enemy wave spawner for shoot-em-up

// Create event
wave = 0;
enemies_per_wave = 5;
spawn_delay = 60;
alarm[0] = 30;
enemies_spawned = 0;
wave_active = false;
enemy_types = [obj_enemy_basic, obj_enemy_fast, obj_enemy_tank];

// Alarm 0 - start next wave
alarm[0] = function() {
    wave++;
    wave_active = true;
    enemies_spawned = 0;
    enemies_per_wave = 5 + wave * 2;
    spawn_delay = max(15, 60 - wave * 3);
    alarm[1] = 10;
}

// Alarm 1 - spawn enemy
alarm[1] = function() {
    if (enemies_spawned < enemies_per_wave) {
        // Pick enemy type based on wave
        var _type_idx = 0;
        if (wave >= 3) _type_idx = 1;
        if (wave >= 5) _type_idx = 2;
        var _type = enemy_types[_type_idx];

        var _sx = random_range(32, room_width - 32);
        var _inst = instance_create_layer(_sx, -32, "Enemies", _type);
        with (_inst) {
            speed = 2 + random(2);
            direction = 90 + random_range(-30, 30);
        }
        enemies_spawned++;
        alarm[1] = spawn_delay;
    } else {
        wave_active = false;
        alarm[0] = 120;
    }
}
