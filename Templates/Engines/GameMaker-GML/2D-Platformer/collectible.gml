/// @description Coin collectible for 2D platformer

// Create event
value = 10;
pickup_effect = function() {
    audio_play_sound(snd_coin, 1, false);
};

// Animation
image_speed = 0.2;

// Step event - bob up and down
z = sin(current_time * 0.004) * 4;
y_start = y;
y = y_start + z;
