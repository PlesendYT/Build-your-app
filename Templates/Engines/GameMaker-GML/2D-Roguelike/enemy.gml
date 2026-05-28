/// @description Roguelike enemy (turn-based)

// Create event
hp = 3 + global.dungeon_level;
max_hp = hp;
atk = 1 + floor(global.dungeon_level / 2);
def = 0;
xp_value = 5 + global.dungeon_level * 2;
move_speed = 1;

function take_turn() {
    if (hp <= 0) {
        instance_destroy();
        return;
    }

    // Simple AI: move toward player if in range
    var _dist = distance_to_object(obj_player);
    if (_dist < 8 * 64) {
        var _dx = sign(obj_player.x - x);
        var _dy = sign(obj_player.y - y);

        var _nx = x + _dx * 64;
        var _ny = y + _dy * 64;

        // Check if player is adjacent
        if (_dist < 96) {
            // Attack player
            var _dmg = max(1, atk + irandom_range(-1, 1) - obj_player.def);
            with (obj_player) {
                hp -= _dmg;
                if (hp <= 0) {
                    game_restart();
                }
            }
        } else {
            // Move toward player
            var _tile = global.dungeon_grid[_nx div 64][_ny div 64];
            var _blocked = instance_position(_nx, _ny, obj_enemy);
            if ((_tile == 2 || _tile == 3) && _blocked == noone && _nx != obj_player.x && _ny != obj_player.y) {
                x = _nx;
                y = _ny;
            }
        }
    }
}
