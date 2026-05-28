/// @description Note manager for rhythm game

// Create event
lanes = [64, 192, 320, 448];
keys = [ord("D"), ord("F"), ord("J"), ord("K")];
notes = []; // Array of active notes
scroll_speed = 4;
beat_interval = 60; // frames per beat (at 120 BPM = 30)
song_position = 0;
current_beat = 0;
is_playing = false;
note_data = [];

// Song data structure: [beat, lane, type]
note_data = [
    [4, 0, "normal"], [4, 2, "normal"],
    [8, 1, "normal"], [8, 3, "normal"],
    [12, 0, "normal"], [12, 1, "normal"],
    [16, 2, "normal"], [16, 3, "normal"],
    [18, 1, "hold"], [20, 1, "hold_end"],
    [24, 0, "normal"], [24, 2, "normal"],
    [28, 1, "normal"], [28, 3, "normal"],
    [32, 0, "normal"], [32, 1, "normal"], [32, 2, "normal"], [32, 3, "normal"],
];

// Step event
if (!is_playing) exit;

song_position++;
current_beat = floor(song_position / beat_interval);

// Spawn notes based on beat data
for (var i = 0; i < array_length(note_data); i++) {
    var _note = note_data[i];
    if (_note[0] == current_beat) {
        var _inst = instance_create_layer(lanes[_note[1]], -32, "Notes", obj_note);
        with (_inst) {
            lane = _note[1];
            note_type = _note[2];
            target_y = 544;
        }
    }
}

// Handle input
for (var l = 0; l < 4; l++) {
    if (keyboard_check_pressed(keys[l])) {
        // Find closest note in this lane
        var _closest = noone;
        var _closest_dist = 100;
        with (obj_note) {
            if (lane == l && !is_hit) {
                var _dist = abs(y - target_y);
                if (_dist < _closest_dist) {
                    _closest_dist = _dist;
                    _closest = id;
                }
            }
        }

        if (_closest != noone) {
            with (_closest) {
                hit_timing = _closest_dist;
                is_hit = true;
                // Rating
                if (_closest_dist < 8) rating = "PERFECT";
                else if (_closest_dist < 16) rating = "GREAT";
                else if (_closest_dist < 32) rating = "GOOD";
                else rating = "BAD";
            }
        }
    }
}

// Draw GUI
draw_set_color(c_white);
draw_text(16, 16, "Score: " + string(obj_score_manager.score));
draw_text(16, 40, "Combo: " + string(obj_score_manager.combo));
