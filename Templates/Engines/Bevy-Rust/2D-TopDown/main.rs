//! A 2D top-down template with player movement, dash ability, and patrolling enemies.

use bevy::prelude::*;

/// Entry point: builds and runs the Bevy app.
fn main() {
    App::new()
        .add_plugins(DefaultPlugins)
        .add_systems(Startup, setup)
        .add_systems(Update, (player_movement, player_dash, enemy_ai, dash_cooldown))
        .run();
}

/// Player component with movement and dash stats.
#[derive(Component)]
struct Player {
    speed: f32,
    dash_speed: f32,
    dash_duration: f32,
    facing: Vec3,
}

/// Enemy component with patrol waypoints and aggro range.
#[derive(Component)]
struct Enemy {
    speed: f32,
    patrol_points: Vec<Vec3>,
    current_target: usize,
    aggro_range: f32,
}

/// Dash ability state: timer, cooldown, active/ready flags.
#[derive(Component)]
struct Dash {
    timer: f32,
    cooldown: f32,
    active: bool,
    ready: bool,
}

/// Velocity component for movement.
#[derive(Component)]
struct Velocity(Vec3);

/// Spawns camera, player with dash, and one patrolling enemy.
fn setup(mut commands: Commands, asset_server: Res<AssetServer>) {
    commands.spawn(Camera2dBundle::default());

    // Player at origin.
    commands.spawn((
        SpriteBundle {
            texture: asset_server.load("player.png"),
            transform: Transform::from_xyz(0.0, 0.0, 0.0),
            ..default()
        },
        Player {
            speed: 200.0,
            dash_speed: 600.0,
            dash_duration: 0.2,
            facing: Vec3::ZERO,
        },
        Dash {
            timer: 0.0,
            cooldown: 0.0,
            active: false,
            ready: true,
        },
        Velocity(Vec3::ZERO),
    ));

    // Enemy with a rectangular patrol path.
    commands.spawn((
        SpriteBundle {
            texture: asset_server.load("enemy.png"),
            transform: Transform::from_xyz(300.0, 0.0, 0.0),
            ..default()
        },
        Enemy {
            speed: 80.0,
            patrol_points: vec![
                Vec3::new(300.0, 0.0, 0.0),
                Vec3::new(500.0, 0.0, 0.0),
                Vec3::new(500.0, 200.0, 0.0),
                Vec3::new(300.0, 200.0, 0.0),
            ],
            current_target: 0,
            aggro_range: 200.0,
        },
        Velocity(Vec3::ZERO),
    ));
}

/// Arrow-key movement; sets velocity on the player. Disabled during dash.
fn player_movement(
    keyboard: Res<ButtonInput<KeyCode>>,
    time: Res<Time>,
    mut query: Query<(&mut Velocity, &mut Player, &Dash), (With<Player>, Without<Enemy>)>,
) {
    for (mut velocity, mut player, dash) in query.iter_mut() {
        if dash.active {
            return;
        }

        let mut direction = Vec3::ZERO;
        if keyboard.pressed(KeyCode::ArrowLeft) { direction.x -= 1.0; }
        if keyboard.pressed(KeyCode::ArrowRight) { direction.x += 1.0; }
        if keyboard.pressed(KeyCode::ArrowUp) { direction.y -= 1.0; }
        if keyboard.pressed(KeyCode::ArrowDown) { direction.y += 1.0; }

        if direction != Vec3::ZERO {
            direction = direction.normalize();
            player.facing = direction;
            velocity.0 = direction * player.speed;
        } else {
            velocity.0 = Vec3::ZERO;
        }
    }
}

/// Triggers a dash in the player's facing direction on Shift press.
fn player_dash(
    keyboard: Res<ButtonInput<KeyCode>>,
    mut query: Query<(&mut Velocity, &Player, &mut Dash)>,
) {
    for (mut velocity, player, mut dash) in query.iter_mut() {
        if keyboard.just_pressed(KeyCode::LShift) && dash.ready && !dash.active {
            dash.active = true;
            dash.ready = false;
            dash.timer = player.dash_duration;
            dash.cooldown = 1.0;
            velocity.0 = player.facing * player.dash_speed;
        }
    }
}

/// Manages dash timer expiry and cooldown recovery.
fn dash_cooldown(time: Res<Time>, mut query: Query<&mut Dash>) {
    for mut dash in query.iter_mut() {
        if dash.active {
            dash.timer -= time.delta_seconds();
            if dash.timer <= 0.0 {
                dash.active = false;
            }
        }
        if !dash.ready {
            dash.cooldown -= time.delta_seconds();
            if dash.cooldown <= 0.0 {
                dash.ready = true;
            }
        }
    }
}

/// Enemy behaviour: chase player when within aggro range, otherwise patrol waypoints.
fn enemy_ai(
    mut query: Query<(&mut Velocity, &mut Enemy, &Transform)>,
    player_query: Query<&Transform, (With<Player>, Without<Enemy>)>,
) {
    let player_pos = if let Some(pos) = player_query.iter().next() {
        pos.translation
    } else {
        return;
    };

    for (mut velocity, mut enemy, transform) in query.iter_mut() {
        let distance = transform.translation.distance(player_pos);

        if distance < enemy.aggro_range {
            // Chase the player at 1.5x speed.
            let direction = (player_pos - transform.translation).normalize();
            velocity.0 = direction * enemy.speed * 1.5;
        } else {
            // Patrol between waypoints.
            let target = enemy.patrol_points[enemy.current_target];
            let direction = (target - transform.translation).normalize();
            velocity.0 = direction * enemy.speed;

            if transform.translation.distance(target) < 10.0 {
                enemy.current_target = (enemy.current_target + 1) % enemy.patrol_points.len();
            }
        }
    }
}
