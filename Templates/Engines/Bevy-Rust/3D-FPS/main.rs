//! A 3D FPS template with mouse-look, WASD movement, shooting, and enemy AI.

use bevy::prelude::*;

/// Entry point: builds and runs the Bevy app.
fn main() {
    App::new()
        .add_plugins(DefaultPlugins)
        .add_systems(Startup, setup)
        .add_systems(Update, (player_look, player_move, shooting, enemy_ai))
        .run();
}

/// Player component with movement and ammo stats.
#[derive(Component)]
struct Player {
    speed: f32,
    health: i32,
    ammo: i32,
    max_ammo: i32,
}

/// Enemy component with health, speed, attack range, and cooldown.
#[derive(Component)]
struct Enemy {
    health: i32,
    speed: f32,
    attack_range: f32,
    attack_cooldown: f32,
    timer: f32,
}

/// Marker component for the gun (placeholder).
#[derive(Component)]
struct Gun;

/// Tracks the player's look direction (pitch/yaw).
#[derive(Component)]
struct LookState {
    pitch: f32,
    yaw: f32,
}

/// Spawns the player camera, light, and two enemies.
fn setup(
    mut commands: Commands,
    mut meshes: ResMut<Assets<Mesh>>,
    mut materials: ResMut<Assets<StandardMaterial>>,
) {
    // Player camera with look state at eye height.
    commands.spawn((
        Camera3dBundle {
            transform: Transform::from_xyz(0.0, 1.5, 0.0),
            ..default()
        },
        Player {
            speed: 5.0,
            health: 100,
            ammo: 30,
            max_ammo: 30,
        },
        LookState {
            pitch: 0.0,
            yaw: 0.0,
        },
    ));

    // Global point light.
    commands.spawn(PointLightBundle {
        transform: Transform::from_xyz(0.0, 10.0, 0.0),
        ..default()
    });

    // Red enemy.
    commands.spawn((
        PbrBundle {
            mesh: meshes.add(Cuboid::from_size(Vec3::splat(0.5))),
            material: materials.add(Color::RED),
            transform: Transform::from_xyz(5.0, 0.5, 5.0),
            ..default()
        },
        Enemy {
            health: 50,
            speed: 2.0,
            attack_range: 10.0,
            attack_cooldown: 2.0,
            timer: 0.0,
        },
    ));

    // Green enemy.
    commands.spawn((
        PbrBundle {
            mesh: meshes.add(Cuboid::from_size(Vec3::splat(0.5))),
            material: materials.add(Color::GREEN),
            transform: Transform::from_xyz(-5.0, 0.5, -5.0),
            ..default()
        },
        Enemy {
            health: 30,
            speed: 3.0,
            attack_range: 8.0,
            attack_cooldown: 1.5,
            timer: 0.0,
        },
    ));
}

/// Updates the camera rotation based on mouse movement.
fn player_look(
    mouse: Res<ButtonInput<MouseButton>>,
    mut mouse_events: EventReader<CursorMoved>,
    mut query: Query<(&mut LookState, &mut Transform), With<Camera3d>>,
) {
    let mut delta = Vec2::ZERO;
    for event in mouse_events.read() {
        delta = event.delta;
    }

    for (mut look, mut transform) in query.iter_mut() {
        look.yaw -= delta.x * 0.003;
        look.pitch -= delta.y * 0.003;
        look.pitch = look.pitch.clamp(-1.5, 1.5);

        transform.rotation = Quat::from_euler(EulerRot::YXZ, look.yaw, look.pitch, 0.0);
    }
}

/// Moves the player forward/back/strafe relative to the look direction.
fn player_move(
    keyboard: Res<ButtonInput<KeyCode>>,
    mut query: Query<(&mut Transform, &LookState), (With<Player>, Without<Enemy>)>,
    time: Res<Time>,
) {
    for (mut transform, look) in query.iter_mut() {
        let mut direction = Vec3::ZERO;

        let forward = Quat::from_rotation_y(look.yaw) * Vec3::NEG_Z;
        let right = Quat::from_rotation_y(look.yaw) * Vec3::X;

        if keyboard.pressed(KeyCode::KeyW) { direction += forward; }
        if keyboard.pressed(KeyCode::KeyS) { direction -= forward; }
        if keyboard.pressed(KeyCode::KeyA) { direction -= right; }
        if keyboard.pressed(KeyCode::KeyD) { direction += right; }

        if direction != Vec3::ZERO {
            direction = direction.normalize();
            transform.translation += direction * 5.0 * time.delta_seconds();
        }
    }
}

/// Fires a ray from the centre of the viewport and marks hit enemies.
fn shooting(
    mouse: Res<ButtonInput<MouseButton>>,
    mut commands: Commands,
    mut meshes: ResMut<Assets<Mesh>>,
    mut materials: ResMut<Assets<StandardMaterial>>,
    camera_query: Query<(&GlobalTransform, &Camera), With<Player>>,
    mut player_query: Query<&mut Player>,
    enemy_query: Query<(Entity, &GlobalTransform, &Enemy)>,
) {
    if !mouse.just_pressed(MouseButton::Left) {
        return;
    }

    let (camera_transform, camera) = camera_query.single();
    let mut player = player_query.single_mut();
    if player.ammo <= 0 {
        return;
    }
    player.ammo -= 1;

    // Cast a ray from the centre of the viewport.
    let ray = camera.viewport_to_world(camera_transform, Vec2::new(0.0, 0.0)).unwrap();
    let ray = Ray3d::new(camera_transform.translation, ray.direction);

    // Check each enemy for a hit using a dot-product threshold.
    for (entity, enemy_transform, enemy) in enemy_query.iter() {
        let enemy_pos = enemy_transform.translation;
        let distance = ray.origin.distance(enemy_pos);
        if distance < 20.0 {
            let to_enemy = (enemy_pos - ray.origin).normalize();
            let dot = ray.direction.dot(to_enemy);
            if dot > 0.95 {
                commands.entity(entity).insert(Hit);
                println!("Enemy hit!");
            }
        }
    }
}

/// Temporary component added to enemies that have been shot (placeholder).
#[derive(Component)]
struct Hit;

/// Chases the player if outside attack range; attacks (logs) when within range.
fn enemy_ai(
    mut commands: Commands,
    time: Res<Time>,
    mut enemy_query: Query<(&mut Enemy, &mut Transform, Entity)>,
    player_query: Query<&Transform, (With<Player>, Without<Enemy>)>,
) {
    let player_pos = player_query.single().translation;

    for (mut enemy, mut transform, entity) in enemy_query.iter_mut() {
        let distance = transform.translation.distance(player_pos);
        let direction = (player_pos - transform.translation).normalize();

        if distance > enemy.attack_range {
            transform.translation += direction * enemy.speed * time.delta_seconds();
        } else {
            enemy.timer += time.delta_seconds();
            if enemy.timer >= enemy.attack_cooldown {
                enemy.timer = 0.0;
                println!("Enemy attacks!");
            }
        }

        transform.look_at(Vec3::new(player_pos.x, transform.translation.y, player_pos.z), Vec3::Y);
    }
}
