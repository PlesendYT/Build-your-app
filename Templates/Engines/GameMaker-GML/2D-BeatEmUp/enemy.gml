/// @description Enemy for beat 'em up

// Create event
hp = 5;
max_hp = 5;
move_speed = 2;
damage = 1;
state = "idle";
aggro_range = 300;
attack_range = 40;
attack_cooldown = 0;
knockback_x = 0;
knockback_y = 0;
enemy_type = "grunt"; // "grunt", "brute"
xp_value = 10;

// Step event
if (hp <= 0) {
    // Death animation
    image_speed = 0.1;
    if (image_index >= image_number - 1) {
        instance_destroy();
    }
    exit;
}

// Apply knockback
if (knockback_x != 0 || knockback_y != 0) {
    x += knockback_x;
    y += knockback_y;
    knockback_x *= 0.8;
    knockback_y *= 0.8;
    if (abs(knockback_x) < 0.5) knockback_x = 0;
    if (abs(knockback_y) < 0.5) knockback_y = 0;
    exit;
}

if (attack_cooldown > 0) {
    attack_cooldown--;
}

// AI
var _dist = distance_to_object(obj_player);
var _dir = sign(obj_player.x - x);
image_xscale = _dir;

if (_dist < aggro_range) {
    if (_dist > attack_range) {
        // Chase player
        x += _dir * move_speed;
        state = "walk";
    } else {
        // Attack
        state = "attack";
        if (attack_cooldown == 0) {
            attack_cooldown = 30;
            // Deal damage to player
            with (obj_player) {
                if (invincible == 0) {
                    hp -= other.damage;
                    invincible = 20;
                    state = "hit";
                }
            }
        }
    }
}

// Take damage function
function take_damage(_dmg, _knockback_dir, _knockback_power) {
    hp -= _dmg;
    knockback_x = _knockback_dir * _knockback_power;
    state = "hit";
    image_speed = 0;
    image_index = 0;
    if (hp <= 0) {
        sprite_index = spr_enemy_death;
    }
}

// Draw health bar
draw_set_color(c_red);
draw_rectangle(x - 16, y - 32, x + 16, y - 28, false);
draw_set_color(c_green);
var _hp_ratio = hp / max_hp;
draw_rectangle(x - 16, y - 32, x - 16 + 32 * _hp_ratio, y - 28, false);
