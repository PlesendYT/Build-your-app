/// @description Tower for tower defense

// Create event
tower_type = "cannon"; // "cannon", "frost", "fire"
range = 128;
fire_rate = 30;
fire_cooldown = 0;
damage = 10;
level = 1;
upgrade_cost = 50;
splash_radius = 0;
slow_amount = 0;

// Step event
if (fire_cooldown > 0) {
    fire_cooldown--;
}

// Find target in range
var _target = noone;
var _nearest_dist = range;
with (obj_enemy) {
    var _d = distance_to_object(other);
    if (_d < other.range && _d < _nearest_dist) {
        _nearest_dist = _d;
        _target = id;
    }
}

if (_target != noone && fire_cooldown == 0) {
    fire_cooldown = fire_rate;

    // Create projectile
    var _proj = instance_create_layer(x, y, "Projectiles", obj_projectile);
    with (_proj) {
        target = _target;
        spd = 8;
        dmg = other.damage;
        splash = other.splash_radius;
        slow = other.slow_amount;
        tower_type = other.tower_type;
    }
}

// Upgrade function
function upgrade_tower() {
    if (level < 3) {
        level++;
        damage += 5;
        range += 16;
        fire_rate = max(8, fire_rate - 3);
        upgrade_cost = ceil(upgrade_cost * 1.5);
    }
}
