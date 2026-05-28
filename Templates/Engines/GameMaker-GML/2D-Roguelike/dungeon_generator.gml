/// @description Procedural dungeon generator

// Create event
tile_size = 64;
grid_width = 40;
grid_height = 30;
dungeon_level = 1;
grid = array_create_2d(grid_width, grid_height, WALL);
seed = current_time;
random_set_seed(seed);

enum TILE {
    EMPTY = 0,
    WALL = 1,
    FLOOR = 2,
    CORRIDOR = 3,
    DOOR = 4
}

// Room struct
function Room(_x, _y, _w, _h) : x = _x, y = _y, w = _w, h = _h {}

function generate_dungeon() {
    // Clear grid
    for (var xx = 0; xx < grid_width; xx++) {
        for (var yy = 0; yy < grid_height; yy++) {
            grid[xx][yy] = TILE.WALL;
        }
    }

    var _rooms = [];

    // Place rooms using simple random placement
    var _attempts = 50;
    var _min_size = 4;
    var _max_size = 10;
    var _placed = 0;

    repeat (_attempts) {
        var _rw = irandom_range(_min_size, _max_size);
        var _rh = irandom_range(_min_size, _max_size);
        var _rx = irandom_range(1, grid_width - _rw - 1);
        var _ry = irandom_range(1, grid_height - _rh - 1);

        // Check overlap
        var _overlap = false;
        for (var i = 0; i < array_length(_rooms); i++) {
            var _r = _rooms[i];
            if (rect_intersect(_rx, _ry, _rx + _rw, _ry + _rh,
                _r.x - 1, _r.y - 1, _r.x + _r.w + 1, _r.y + _r.h + 1)) {
                _overlap = true;
                break;
            }
        }

        if (!_overlap) {
            array_push(_rooms, new Room(_rx, _ry, _rw, _rh));
            carv_room(_rx, _ry, _rw, _rh);
            _placed++;
        }
    }

    // Connect rooms with corridors
    for (var i = 1; i < array_length(_rooms); i++) {
        var _a = _rooms[i - 1];
        var _b = _rooms[i];
        var _ax = _a.x + floor(_a.w / 2);
        var _ay = _a.y + floor(_a.h / 2);
        var _bx = _b.x + floor(_b.w / 2);
        var _by = _b.y + floor(_b.h / 2);

        // L-shaped corridor
        carve_h_corridor(_ax, _bx, _ay);
        carve_v_corridor(_ay, _by, _bx);
    }

    // Place stairs
    var _last_room = _rooms[array_length(_rooms) - 1];
    var _sx = _last_room.x + floor(_last_room.w / 2);
    var _sy = _last_room.y + floor(_last_room.h / 2);
    instance_create_layer(_sx * tile_size, _sy * tile_size, "Instances", obj_stairs);

    // Place player in first room
    var _first_room = _rooms[0];
    obj_player.x = (_first_room.x + floor(_first_room.w / 2)) * tile_size;
    obj_player.y = (_first_room.y + floor(_first_room.h / 2)) * tile_size;
}

function carv_room(_x, _y, _w, _h) {
    for (var xx = _x; xx < _x + _w; xx++) {
        for (var yy = _y; yy < _y + _h; yy++) {
            grid[xx][yy] = TILE.FLOOR;
        }
    }
}

function carve_h_corridor(_x1, _x2, _y) {
    for (var xx = min(_x1, _x2); xx <= max(_x1, _x2); xx++) {
        grid[xx][_y] = TILE.CORRIDOR;
        grid[xx][_y + 1] = TILE.CORRIDOR;
    }
}

function carve_v_corridor(_y1, _y2, _x) {
    for (var yy = min(_y1, _y2); yy <= max(_y1, _y2); yy++) {
        grid[_x][yy] = TILE.CORRIDOR;
        grid[_x + 1][yy] = TILE.CORRIDOR;
    }
}

function rect_intersect(_x1, _y1, _x2, _y2, _x3, _y3, _x4, _y4) {
    return _x1 < _x4 && _x2 > _x3 && _y1 < _y4 && _y2 > _y3;
}

function build_tilemap() {
    // Convert grid to visual tilemap
    for (var xx = 0; xx < grid_width; xx++) {
        for (var yy = 0; yy < grid_height; yy++) {
            var _px = xx * tile_size;
            var _py = yy * tile_size;
            if (grid[xx][yy] == TILE.WALL) {
                instance_create_layer(_px, _py, "Walls", obj_wall);
            } else {
                instance_create_layer(_px, _py, "Floors", obj_floor);
            }
        }
    }
}
