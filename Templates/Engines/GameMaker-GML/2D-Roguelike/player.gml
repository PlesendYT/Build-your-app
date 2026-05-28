/// @description Roguelike player (turn-based)

// Create event
hp = 10;
max_hp = 10;
atk = 2;
def = 1;
level = 1;
xp = 0;
xp_next = 10;
turns = 0;
fov_radius = 6;

// Step event
if (!global.player_turn) exit;

var _dx = 0;
var _dy = 0;

if (keyboard_check_pressed(vk_right))  _dx = 1;
if (keyboard_check_pressed(vk_left))   _dx = -1;
if (keyboard_check_pressed(vk_down))   _dy = 1;
if (keyboard_check_pressed(vk_up))     _dy = -1;
if (keyboard_check_pressed(ord("G")))  pickup_item();
if (keyboard_check_pressed(ord(">")))  descend_stairs();

if (_dx != 0 || _dy != 0) {
    var _nx = x + _dx * 64;
    var _ny = y + _dy * 64;

    // Check walls
    var _tile = global.dungeon_grid[_nx div 64][_ny div 64];
    if (_tile == 2 || _tile == 3) { // FLOOR or CORRIDOR
        // Check for enemy at target
        var _enemy = instance_position(_nx, _ny, obj_enemy);
        if (_enemy != noone) {
            attack_enemy(_enemy);
        } else {
            x = _nx;
            y = _ny;
        }
        end_turn();
    }
}

function attack_enemy(_enemy) {
    var _dmg = max(1, atk + irandom_range(-1, 2) - _enemy.def);
    with (_enemy) {
        hp -= _dmg;
        if (hp <= 0) {
            obj_player.xp += xp_value;
            instance_destroy();
        }
    }
}

function end_turn() {
    turns++;
    global.player_turn = false;
    // Enemy turn
    with (obj_enemy) {
        take_turn();
    }
    global.player_turn = true;
}

function pickup_item() {
    var _item = instance_position(x, y, obj_item);
    if (_item != noone) {
        with (_item) { pickup(); }
    }
}

function descend_stairs() {
    if (instance_position(x, y, obj_stairs) != noone) {
        global.dungeon_level++;
        obj_dungeon_gen.generate_dungeon();
    }
}

// Draw event - fog of war (simplified)
draw_set_alpha(0.5);
for (var xx = 0; xx < 40; xx++) {
    for (var yy = 0; yy < 30; yy++) {
        var _dist = point_distance(x, y, xx * 64 + 32, yy * 64 + 32);
        if (_dist > fov_radius * 64) {
            draw_set_color(c_black);
            draw_rectangle(xx * 64, yy * 64, xx * 64 + 64, yy * 64 + 64, false);
        }
    }
}
draw_set_alpha(1.0);
