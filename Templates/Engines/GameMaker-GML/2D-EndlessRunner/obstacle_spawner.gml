/// @description Obstacle spawner for endless runner

// Create event
alarm[0] = 60;
min_interval = 30;
max_interval = 90;
obstacle_types = ["low", "high", "double"];

// Alarm 0 - spawn obstacle
alarm[0] = function() {
    if (obj_player.game_over) exit;

    var _type = obstacle_types[irandom(array_length(obstacle_types) - 1)];
    var _ox = room_width + 64;
    var _oy = room_height - 64;

    switch (_type) {
        case "low":
            instance_create_layer(_ox, _oy, "Obstacles", obj_obstacle_low);
            break;
        case "high":
            instance_create_layer(_ox, _oy - 48, "Obstacles", obj_obstacle_high);
            break;
        case "double":
            instance_create_layer(_ox, _oy, "Obstacles", obj_obstacle_low);
            instance_create_layer(_ox + 48, _oy - 48, "Obstacles", obj_obstacle_high);
            break;
    }

    // Set next spawn
    var _interval = irandom_range(max(15, min_interval - obj_player.speed_mult * 2),
                                   max(30, max_interval - obj_player.speed_mult * 3));
    alarm[0] = _interval;
}

// Step event - destroy obstacles that leave the screen
with (obj_obstacle) {
    if (x < -64) {
        instance_destroy();
    }
}
