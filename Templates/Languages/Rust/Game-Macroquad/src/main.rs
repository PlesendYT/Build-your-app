//! A simple coin-collection game built with Macroquad.
//! The player moves with arrow keys and collects gold coins.

use macroquad::prelude::*;

/// Entry point: initialises the window and runs the game loop.
#[macroquad::main("Macroquad Game")]
async fn main() {
    // Player starts at the centre of the screen.
    let mut player_pos = Vec2::new(
        screen_width() / 2.0,
        screen_height() / 2.0,
    );
    let player_size = 40.0;
    let mut score = 0u32;

    // Spawn 8 coins at random positions.
    let mut coins: Vec<Vec2> = (0..8)
        .map(|_| {
            vec2(
                rand::random::<f32>() * screen_width(),
                rand::random::<f32>() * screen_height(),
            )
        })
        .collect();

    loop {
        clear_background(DARKGRAY);

        // Handle arrow-key movement with frame-rate-independent speed.
        let speed = 300.0 * get_frame_time();
        if is_key_down(KeyCode::Left) { player_pos.x -= speed; }
        if is_key_down(KeyCode::Right) { player_pos.x += speed; }
        if is_key_down(KeyCode::Up) { player_pos.y -= speed; }
        if is_key_down(KeyCode::Down) { player_pos.y += speed; }

        // Clamp player position inside the screen bounds.
        player_pos.x = player_pos.x.clamp(0.0, screen_width());
        player_pos.y = player_pos.y.clamp(0.0, screen_height());

        // Draw the player as a blue circle.
        draw_circle(player_pos.x, player_pos.y, player_size / 2.0, BLUE);

        // Check coin collisions; collected coins are removed.
        coins.retain(|&coin| {
            let dist = player_pos.distance(coin);
            if dist < player_size / 2.0 + 10.0 {
                score += 10;
                false
            } else {
                draw_circle(coin.x, coin.y, 10.0, GOLD);
                true
            }
        });

        // Show a win message when all coins are collected.
        if coins.is_empty() {
            draw_text("You Win!", screen_width() / 2.0 - 60.0, screen_height() / 2.0, 40.0, GREEN);
        }

        // Display the current score.
        draw_text(&format!("Score: {score}"), 10.0, 30.0, 30.0, WHITE);

        next_frame().await;
    }
}
