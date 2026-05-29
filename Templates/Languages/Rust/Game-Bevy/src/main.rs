//! A simple Bevy game where the player collects enemies for points.

use bevy::prelude::*;

/// Marker component for the player, with movement speed.
#[derive(Component)]
struct Player {
    speed: f32,
}

/// Marker component for enemies the player can collect.
#[derive(Component)]
struct Enemy;

/// Global score resource, incremented when an enemy is collected.
#[derive(Resource)]
struct Score(u32);

/// Spawns the camera, player, and initial set of enemies.
fn setup(mut commands: Commands) {
    commands.spawn(Camera2d);
    commands.insert_resource(Score(0));

    // Spawn the player as a blue square at the origin.
    commands.spawn((
        Sprite {
            color: Color::srgb(0.2, 0.6, 1.0),
            custom_size: Some(Vec2::new(50.0, 50.0)),
            ..default()
        },
        Transform::from_xyz(0.0, 0.0, 0.0),
        Player { speed: 300.0 },
    ));

    // Spawn 5 red enemy squares at random positions.
    for _ in 0..5 {
        let x = rand::random::<f32>() * 600.0 - 300.0;
        let y = rand::random::<f32>() * 600.0 - 300.0;
        commands.spawn((
            Sprite {
                color: Color::srgb(1.0, 0.2, 0.2),
                custom_size: Some(Vec2::new(30.0, 30.0)),
                ..default()
            },
            Transform::from_xyz(x, y, 0.0),
            Enemy,
        ));
    }
}

/// Moves the player based on arrow-key input, frame-rate independent.
fn player_movement(
    time: Res<Time>,
    keyboard: Res<ButtonInput<KeyCode>>,
    mut query: Query<(&mut Transform, &Player)>,
) {
    let (mut transform, player) = query.single_mut();
    let mut dx = 0.0;
    let mut dy = 0.0;

    if keyboard.pressed(KeyCode::ArrowLeft) { dx -= 1.0; }
    if keyboard.pressed(KeyCode::ArrowRight) { dx += 1.0; }
    if keyboard.pressed(KeyCode::ArrowUp) { dy += 1.0; }
    if keyboard.pressed(KeyCode::ArrowDown) { dy -= 1.0; }

    let delta = Vec2::new(dx, dy).normalize_or_zero() * player.speed * time.delta_secs();
    transform.translation.x += delta.x;
    transform.translation.y += delta.y;
}

/// Despawns enemies within range of the player and adds to the score.
fn collect_enemies(
    mut commands: Commands,
    player: Query<&Transform, With<Player>>,
    enemies: Query<(Entity, &Transform), With<Enemy>>,
    mut score: ResMut<Score>,
) {
    let player_pos = player.single().translation;
    for (entity, enemy_pos) in &enemies {
        let dist = player_pos.distance(enemy_pos.translation);
        if dist < 40.0 {
            commands.entity(entity).despawn();
            score.0 += 10;
            println!("Score: {}", score.0);
        }
    }
}

/// Prints the current score to the console each frame.
fn score_display(score: Res<Score>) {
    println!("Score: {}", score.0);
}

/// Entry point: builds and runs the Bevy app.
fn main() {
    App::new()
        .add_plugins(DefaultPlugins)
        .add_systems(Startup, setup)
        .add_systems(Update, (player_movement, collect_enemies, score_display))
        .run();
}
