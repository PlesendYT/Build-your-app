/// @description Wave manager for tower defense

// Create event
wave = 0;
gold = 100;
lives = 20;
max_lives = 20;
enemies_spawned = 0;
enemies_per_wave = 5;
spawn_delay = 30;
wave_active = false;
between_waves = true;

// Waves definition
wave_data = [];
wave_data[1] = { enemy: obj_enemy_basic, count: 5, interval: 30, hp_mult: 1.0 };
wave_data[2] = { enemy: obj_enemy_basic, count: 8, interval: 25, hp_mult: 1.2 };
wave_data[3] = { enemy: obj_enemy_fast, count: 10, interval: 20, hp_mult: 1.0 };
wave_data[4] = { enemy: obj_enemy_armored, count: 6, interval: 35, hp_mult: 1.3 };
wave_data[5] = { enemy: obj_enemy_boss, count: 1, interval: 60, hp_mult: 2.0 };

// Start wave function
function start_wave() {
    wave++;
    wave_active = true;
    between_waves = false;
    enemies_spawned = 0;

    // Get wave data or generate scaling
    var _data = undefined;
    if (wave <= array_length(wave_data)) {
        _data = wave_data[wave];
    } else {
        _data = { enemy: obj_enemy_basic, count: 5 + wave * 2, interval: max(10, 30 - wave), hp_mult: 1.0 + wave * 0.1 };
    }
    alarm[0] = 10;
}

// Alarm 0 - spawn enemy
alarm[0] = function() {
    if (!wave_active) exit;

    var _data = wave_data[min(wave, array_length(wave_data))];
    var _wp = obj_waypoints;
    if (_wp == noone) exit;

    // Spawn at first waypoint
    var _inst = instance_create_layer(_wp.x, _wp.y, "Enemies", _data.enemy);
    with (_inst) {
        hp = hp * _data.hp_mult;
        max_hp = hp;
        waypoints = _wp.waypoint_list;
    }
    enemies_spawned++;

    if (enemies_spawned < _data.count) {
        alarm[0] = _data.interval;
    } else {
        wave_active = false;
    }
}

// Step - check if wave is done
if (!wave_active && !between_waves && instance_number(obj_enemy) == 0) {
    between_waves = true;
    // Ready for next wave (player clicks button)
}
