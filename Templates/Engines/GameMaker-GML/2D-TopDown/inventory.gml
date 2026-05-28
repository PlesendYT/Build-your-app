/// @description Inventory system for top-down game

// Create event (persistent controller object)
inventory = [];
max_slots = 10;
selected_slot = 0;
is_open = false;

// Step event
if (keyboard_check_pressed(ord("I"))) {
    is_open = !is_open;
}

// Draw GUI event
if (is_open) {
    var _gw = 200;
    var _gh = 300;
    var _gx = (display_get_gui_width() - _gw) / 2;
    var _gy = (display_get_gui_height() - _gh) / 2;

    draw_set_color(c_black);
    draw_rectangle(_gx, _gy, _gx + _gw, _gy + _gh, false);
    draw_set_color(c_white);
    draw_rectangle(_gx, _gy, _gx + _gw, _gy + _gh, true);

    draw_set_halign(fa_center);
    draw_text(_gx + _gw / 2, _gy + 16, "Inventory");

    for (var i = 0; i < array_length(inventory); i++) {
        var _iy = _gy + 48 + i * 24;
        draw_set_color(selected_slot == i ? c_yellow : c_white);
        draw_text(_gx + 16, _iy, inventory[i]);
    }
}

// Functions
function inventory_add(item_name) {
    if (array_length(inventory) < max_slots) {
        array_push(inventory, item_name);
        return true;
    }
    return false;
}

function inventory_remove(item_name) {
    var _idx = array_get_index(inventory, item_name);
    if (_idx != -1) {
        array_delete(inventory, _idx, 1);
        return true;
    }
    return false;
}

function inventory_has(item_name) {
    return array_contains(inventory, item_name);
}
