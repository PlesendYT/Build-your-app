//! A 2D platformer template with player movement, double-jump, gravity, patrol enemies, and coins.

use bevy::prelude::*;

/// Entry point: builds and runs the Bevy app.
fn main() {
    App::new()
        .add_plugins(DefaultPlugins)
        .add_systems(Startup, setup)
        .add_systems(Update, (player_movement, player_jump, gravity, enemy_patrol, coin_collect))
        .run();
}

/// Player component with movement, jump, and health stats.
#[derive(Component)]
struct Player {
    speed: f32,
    jump_velocity: f32,
    jumps_remaining: u32,
    health: i32,
}

/// Enemy component with patrol boundaries.
#[derive(Component)]
struct Enemy {
    speed: f32,
    start_x: f32,
    patrol_distance: f32,
    moving_right: bool,
}

/// Collectable coin with a point value.
#[derive(Component)]
struct Coin {
    value: i32,
}

/// Velocity applied each frame.
#[derive(Component)]
struct Velocity(Vec3);

/// Tracks whether the player is standing on the ground.
#[derive(Component)]
struct Grounded(bool);

/// Spawns camera, player, enemy, and coins.
fn setup(mut commands: Commands, asset_server: Res<AssetServer>) {
    commands.spawn(Camera2dBundle::default());

    // Player at the origin.
    commands.spawn((
        SpriteBundle {
            texture: asset_server.load("player.png"),
            transform: Transform::from_xyz(0.0, 0.0, 0.0),
            ..default()
        },
        Player {
            speed: 300.0,
            jump_velocity: 400.0,
            jumps_remaining: 2,
            health: 3,
        },
        Velocity(Vec3::ZERO),
        Grounded(false),
    ));

    // Patrol enemy to the right.
    commands.spawn((
        SpriteBundle {
            texture: asset_server.load("enemy.png"),
            transform: Transform::from_xyz(200.0, 0.0, 0.0),
            ..default()
        },
        Enemy {
            speed: 100.0,
            start_x: 200.0,
            patrol_distance: 150.0,
            moving_right: true,
        },
        Velocity(Vec3::ZERO),
    ));

    // Coins placed in a horizontal line.
    for i in 0..5 {
        commands.spawn((
            SpriteBundle {
                texture: asset_server.load("coin.png"),
                transform: Transform::from_xyz(i as f32 * 100.0 + 50.0, 50.0, 0.0),
                ..default()
            },
            Coin { value: 1 },
        ));
    }
}

/// Left/right arrow keys set horizontal velocity.
fn player_movement(
    keyboard: Res<ButtonInput<KeyCode>>,
    mut query: Query<(&mut Velocity, &Player)>,
) {
    for (mut velocity, player) in query.iter_mut() {
        let mut direction = 0.0;
        if keyboard.pressed(KeyCode::ArrowLeft) {
            direction -= 1.0;
        }
        if keyboard.pressed(KeyCode::ArrowRight) {
            direction += 1.0;
        }
        velocity.0.x = direction * player.speed;
    }
}

/// Space triggers a jump (or double-jump if already airborne).
fn player_jump(
    keyboard: Res<ButtonInput<KeyCode>>,
    mut query: Query<(&mut Velocity, &mut Player, &mut Grounded)>,
) {
    for (mut velocity, mut player, mut grounded) in query.iter_mut() {
        if keyboard.just_pressed(KeyCode::Space) {
            if grounded.0 {
                velocity.0.y = -player.jump_velocity;
                player.jumps_remaining = 1;
                grounded.0 = false;
            } else if player.jumps_remaining > 0 {
                velocity.0.y = -player.jump_velocity * 0.9;
                player.jumps_remaining -= 1;
            }
        }
    }
}

/// Applies a simple downward acceleration and resets grounded state at y=0.
fn gravity(
    time: Res<Time>,
    mut query: Query<(&mut Velocity, &mut Grounded, &Transform), With<Player>>,
) {
    for (mut velocity, mut grounded, transform) in query.iter_mut() {
        velocity.0.y += 980.0 * time.delta_seconds();
        if transform.translation.y <= 0.0 {
            grounded.0 = true;
        }
    }
}

/// Moves the enemy back and forth within its patrol range.
fn enemy_patrol(
    mut query: Query<(&mut Velocity, &mut Enemy)>,
) {
    for (mut velocity, mut enemy) in query.iter_mut() {
        if enemy.moving_right {
            velocity.0.x = enemy.speed;
            if enemy.start_x + enemy.patrol_distance <= 0.0 {
                enemy.moving_right = false;
            }
        } else {
            velocity.0.x = -enemy.speed;
        }
    }
}

/// Despawns coins when the player overlaps them.
fn coin_collect(
    mut commands: Commands,
    player_query: Query<&Transform, With<Player>>,
    coin_query: Query<(Entity, &Transform), With<Coin>>,
) {
    for player_transform in player_query.iter() {
        for (coin_entity, coin_transform) in coin_query.iter() {
            let distance = player_transform.translation.distance(coin_transform.translation);
            if distance < 30.0 {
                commands.entity(coin_entity).despawn();
                println!("Coin collected!");
            }
        }
    }
}
