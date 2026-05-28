/// @description Puzzle level manager

// Create event
current_level = 0;
max_levels = 10;
level_data = [];
undo_stack = [];
move_count = 0;

// Initialize levels
level_data[0] = [
    [1, 1, 1, 1, 1],
    [1, 0, 0, 0, 1],
    [1, 0, 2, 3, 1],
    [1, 0, 0, 4, 1],
    [1, 1, 1, 1, 1]
];
// 0=empty, 1=wall, 2=player, 3=block, 4=goal, 5=plate

level_data[1] = [
    [1, 1, 1, 1, 1, 1],
    [1, 2, 0, 3, 4, 1],
    [1, 0, 0, 0, 0, 1],
    [1, 1, 1, 1, 1, 1]
];

function load_level(lvl) {
    // Clear existing objects
    with (obj_block) instance_destroy();
    with (obj_plate) instance_destroy();
    with (obj_door) instance_destroy();
    with (obj_goal) instance_destroy();

    var _data = level_data[lvl];
    for (var yy = 0; yy < array_length(_data); yy++) {
        for (var xx = 0; xx < array_length(_data[yy]); xx++) {
            var _cell = _data[yy][xx];
            var _px = xx * 64 + 32;
            var _py = yy * 64 + 32;

            switch (_cell) {
                case 0: break;
                case 1: instance_create_layer(_px, _py, "Walls", obj_wall); break;
                case 2: obj_player.x = _px; obj_player.y = _py; break;
                case 3: instance_create_layer(_px, _py, "Blocks", obj_block); break;
                case 4: instance_create_layer(_px, _py, "Goals", obj_goal); break;
            }
        }
    }
    undo_stack = [];
    move_count = 0;
}

function check_win() {
    // Check if all blocks are on goal positions
    var _blocks = instance_number(obj_block);
    var _on_goal = 0;
    with (obj_block) {
        if (place_meeting(x, y, obj_goal)) {
            _on_goal++;
        }
    }
    return _blocks > 0 && _on_goal == _blocks;
}

function push_undo() {
    var _state = { px: obj_player.x, py: obj_player.y };
    with (obj_block) {
        array_push(_state, { bx: x, by: y });
    }
    array_push(undo_stack, _state);
}

function undo_move() {
    if (array_length(undo_stack) > 0) {
        var _state = array_pop(undo_stack);
        obj_player.x = _state.px;
        obj_player.y = _state.py;
        var _idx = 0;
        with (obj_block) {
            var _b = _state[++_idx];
            x = _b.bx;
            y = _b.by;
        }
    }
}
