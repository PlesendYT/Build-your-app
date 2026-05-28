/// @description Score manager for rhythm game

// Create event
score = 0;
combo = 0;
max_combo = 0;
perfect_count = 0;
great_count = 0;
good_count = 0;
bad_count = 0;
miss_count = 0;
total_notes = 0;

// Score values
score_values = {
    PERFECT: 300,
    GREAT: 200,
    GOOD: 100,
    BAD: 50,
    MISS: 0
};

// Step event - check notes that were missed
with (obj_note) {
    if (!is_hit && y > target_y + 64) {
        is_hit = true;
        obj_score_manager.miss_note();
        instance_destroy();
    }
}

function hit_note(_rating) {
    total_notes++;
    var _points = score_values[$ _rating];
    combo++;
    if (combo > max_combo) max_combo = combo;

    // Combo multiplier
    var _mult = 1 + floor(combo / 10) * 0.5;
    _points = ceil(_points * _mult);
    score += _points;

    switch (_rating) {
        case "PERFECT": perfect_count++; break;
        case "GREAT": great_count++; break;
        case "GOOD": good_count++; break;
        case "BAD": bad_count++; break;
    }

    audio_play_sound(snd_hit, 1, false);
}

function miss_note() {
    total_notes++;
    miss_count++;
    combo = 0;
    audio_play_sound(snd_miss, 1, false);
}

// Draw results screen
function draw_results() {
    var _cx = room_width / 2;
    var _cy = 120;
    draw_set_halign(fa_center);
    draw_set_color(c_white);
    draw_text(_cx, _cy, "RESULTS");
    draw_text(_cx, _cy + 40, "Score: " + string(score));
    draw_text(_cx, _cy + 70, "Max Combo: " + string(max_combo));
    draw_text(_cx, _cy + 100, "PERFECT: " + string(perfect_count));
    draw_text(_cx, _cy + 125, "GREAT: " + string(great_count));
    draw_text(_cx, _cy + 150, "GOOD: " + string(good_count));
    draw_text(_cx, _cy + 175, "BAD: " + string(bad_count));
    draw_text(_cx, _cy + 200, "MISS: " + string(miss_count));

    var _accuracy = 0;
    if (total_notes > 0) {
        _accuracy = (perfect_count + great_count * 0.8 + good_count * 0.5 + bad_count * 0.2) / total_notes * 100;
    }
    draw_text(_cx, _cy + 240, "Accuracy: " + string_format(_accuracy, 1, 1) + "%");
}
