/// @description Bullet for shoot-em-up

// Create event
speed = 10;
direction = -90;
damage = 1;
is_player_bullet = true;

// Step event
// Destroy when leaving room
if (x < -16 || x > room_width + 16 || y < -16 || y > room_height + 16) {
    instance_destroy();
}

// Collision with enemy
if (is_player_bullet && place_meeting(x, y, obj_enemy)) {
    with (other) {
        hp -= other.damage;
        if (hp <= 0) {
            obj_player.score += 100;
            instance_destroy();
        }
    }
    instance_destroy();
}
