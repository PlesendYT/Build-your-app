use macroquad::prelude::*;

#[macroquad::main("Macroquad Game")]
async fn main() {
    let mut player_pos = Vec2::new(
        screen_width() / 2.0,
        screen_height() / 2.0,
    );
    let player_size = 40.0;
    let mut score = 0u32;
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

        let speed = 300.0 * get_frame_time();
        if is_key_down(KeyCode::Left) { player_pos.x -= speed; }
        if is_key_down(KeyCode::Right) { player_pos.x += speed; }
        if is_key_down(KeyCode::Up) { player_pos.y -= speed; }
        if is_key_down(KeyCode::Down) { player_pos.y += speed; }

        player_pos.x = player_pos.x.clamp(0.0, screen_width());
        player_pos.y = player_pos.y.clamp(0.0, screen_height());

        draw_circle(player_pos.x, player_pos.y, player_size / 2.0, BLUE);

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

        if coins.is_empty() {
            draw_text("You Win!", screen_width() / 2.0 - 60.0, screen_height() / 2.0, 40.0, GREEN);
        }

        draw_text(&format!("Score: {score}"), 10.0, 30.0, 30.0, WHITE);

        next_frame().await;
    }
}
