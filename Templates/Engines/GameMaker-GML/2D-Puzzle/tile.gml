/// @description Pushable block for puzzle game

// Create event
is_moving = false;

// Function to try push
function try_push(_dir_x, _dir_y) {
    var _nx = x + _dir_x * 64;
    var _ny = y + _dir_y * 64;

    // Check if destination is free
    if (!place_meeting(_nx, _ny, obj_wall) && !place_meeting(_nx, _ny, obj_block)) {
        x = _nx;
        y = _ny;
        return true;
    }
    return false;
}
