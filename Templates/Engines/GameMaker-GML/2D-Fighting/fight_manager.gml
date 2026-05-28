/// @description Fight manager for fighting game

// Create event
round = 1;
max_rounds = 3;
p1_wins = 0;
p2_wins = 0;
round_timer = 99;
state = "intro"; // "intro", "fight", "ko", "result"
state_timer = 0;
ko_message = "";

// Step event
switch (state) {
    case "intro":
        state_timer++;
        if (state_timer > 60) {
            state = "fight";
            state_timer = 0;
            round_timer = 99;
            // Reset fighters
            with (obj_fighter) {
                hp = max_hp;
                energy = 0;
                combo_damage = 0;
                if (player_id == 1) { x = 160; y = room_height - 96; }
                else { x = room_width - 160; y = room_height - 96; }
            }
        }
        break;

    case "fight":
        round_timer--;
        if (round_timer <= 0) {
            state = "ko";
            ko_message = "TIME OVER!";
            // Whoever has more HP wins
        }

        // Check KO
        with (obj_fighter) {
            if (hp <= 0) {
                other.state = "ko";
                other.ko_message = (player_id == 1) ? "P2 WINS!" : "P1 WINS!";
                with (other) { state_timer = 0; }
            }
        }
        break;

    case "ko":
        state_timer++;
        if (state_timer > 120) {
            if (p1_wins >= ceil(max_rounds / 2) || p2_wins >= ceil(max_rounds / 2)) {
                state = "result";
            } else {
                round++;
                state = "intro";
                state_timer = 0;
            }
        }
        break;

    case "result":
        // Final result screen
        if (keyboard_check_pressed(vk_space)) {
            game_restart();
        }
        break;
}

// Draw GUI
draw_set_color(c_white);
draw_set_halign(fa_center);

// Health bars
draw_health_bar(32, 32, 300, 20, obj_fighter_P1.hp, obj_fighter_P1.max_hp, c_red, c_lime);
draw_health_bar(room_width - 332, 32, 300, 20, obj_fighter_P2.hp, obj_fighter_P2.max_hp, c_red, c_lime);

// Round display
draw_set_font(fnt_round);
draw_text(room_width / 2, 32, "ROUND " + string(round));
draw_text(room_width / 2, 64, "P1: " + string(p1_wins) + " - P2: " + string(p2_wins));

// Timer
draw_set_font(fnt_timer);
draw_set_color(round_timer < 30 ? c_red : c_white);
draw_text(room_width / 2, 100, string(round_timer));

// KO / Result
if (state == "ko") {
    draw_set_font(fnt_ko);
    draw_set_color(c_yellow);
    draw_text(room_width / 2, room_height / 2, ko_message);
}

function draw_health_bar(_x, _y, _w, _h, _hp, _max_hp, _bg, _fg) {
    draw_set_color(_bg);
    draw_rectangle(_x, _y, _x + _w, _y + _h, false);
    draw_set_color(_fg);
    var _ratio = max(0, _hp / _max_hp);
    draw_rectangle(_x, _y, _x + _w * _ratio, _y + _h, false);
    draw_set_color(c_white);
    draw_rectangle(_x, _y, _x + _w, _y + _h, true);
}

function opponent(_self) {
    with (obj_fighter) {
        if (id != _self) return id;
    }
    return noone;
}
