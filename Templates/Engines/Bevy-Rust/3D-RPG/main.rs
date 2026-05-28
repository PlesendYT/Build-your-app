use bevy::prelude::*;

fn main() {
    App::new()
        .add_plugins(DefaultPlugins)
        .add_systems(Startup, setup)
        .add_systems(Update, (player_move, camera_follow, enemy_ai, combat))
        .run();
}

#[derive(Component)]
struct Player {
    speed: f32,
    health: i32,
    max_health: i32,
    level: i32,
    attack: i32,
}

#[derive(Component)]
struct Enemy {
    health: i32,
    max_health: i32,
    attack: i32,
    speed: f32,
    aggro_range: f32,
    is_alive: bool,
}

#[derive(Component)]
struct MainCamera;

fn setup(
    mut commands: Commands,
    mut meshes: ResMut<Assets<Mesh>>,
    mut materials: ResMut<Assets<StandardMaterial>>,
) {
    commands.spawn((
        PbrBundle {
            mesh: meshes.add(Capsule3d::new(0.4, 1.0)),
            material: materials.add(Color::BLUE),
            transform: Transform::from_xyz(0.0, 1.0, 0.0),
            ..default()
        },
        Player {
            speed: 5.0,
            health: 100,
            max_health: 100,
            level: 1,
            attack: 10,
        },
    ));

    commands.spawn((
        Camera3dBundle {
            transform: Transform::from_xyz(0.0, 5.0, -10.0).looking_at(Vec3::ZERO, Vec3::Y),
            ..default()
        },
        MainCamera,
    ));

    commands.spawn(PointLightBundle {
        transform: Transform::from_xyz(0.0, 10.0, 0.0),
        ..default()
    });

    commands.spawn((
        PbrBundle {
            mesh: meshes.add(Capsule3d::new(0.4, 1.0)),
            material: materials.add(Color::RED),
            transform: Transform::from_xyz(5.0, 1.0, 5.0),
            ..default()
        },
        Enemy {
            health: 30,
            max_health: 30,
            attack: 5,
            speed: 2.0,
            aggro_range: 10.0,
            is_alive: true,
        },
    ));

    commands.spawn((
        PbrBundle {
            mesh: meshes.add(Capsule3d::new(0.4, 1.0)),
            material: materials.add(Color::RED),
            transform: Transform::from_xyz(-5.0, 1.0, -5.0),
            ..default()
        },
        Enemy {
            health: 50,
            max_health: 50,
            attack: 8,
            speed: 1.5,
            aggro_range: 12.0,
            is_alive: true,
        },
    ));
}

fn player_move(
    keyboard: Res<ButtonInput<KeyCode>>,
    mut query: Query<&mut Transform, With<Player>>,
    time: Res<Time>,
) {
    for mut transform in query.iter_mut() {
        let mut direction = Vec3::ZERO;
        if keyboard.pressed(KeyCode::KeyW) { direction.z -= 1.0; }
        if keyboard.pressed(KeyCode::KeyS) { direction.z += 1.0; }
        if keyboard.pressed(KeyCode::KeyA) { direction.x -= 1.0; }
        if keyboard.pressed(KeyCode::KeyD) { direction.x += 1.0; }

        if direction != Vec3::ZERO {
            direction = direction.normalize();
            transform.translation += direction * 5.0 * time.delta_seconds();
            transform.look_at(
                Vec3::new(
                    transform.translation.x + direction.x,
                    transform.translation.y,
                    transform.translation.z + direction.z,
                ),
                Vec3::Y,
            );
        }
    }
}

fn camera_follow(
    player_query: Query<&Transform, With<Player>>,
    mut camera_query: Query<&mut Transform, (With<MainCamera>, Without<Player>)>,
) {
    let player_pos = player_query.single().translation;
    for mut camera_transform in camera_query.iter_mut() {
        let desired = Vec3::new(player_pos.x, player_pos.y + 5.0, player_pos.z - 8.0);
        camera_transform.translation = camera_transform.translation.lerp(desired, 0.1);
        camera_transform.look_at(player_pos, Vec3::Y);
    }
}

fn enemy_ai(
    mut enemy_query: Query<(&mut Enemy, &mut Transform), Without<Player>>,
    player_query: Query<&Transform, (With<Player>, Without<Enemy>)>,
    time: Res<Time>,
) {
    let player_pos = player_query.single().translation;

    for (mut enemy, mut transform) in enemy_query.iter_mut() {
        if !enemy.is_alive {
            continue;
        }

        let distance = transform.translation.distance(player_pos);
        if distance < enemy.aggro_range {
            let direction = (player_pos - transform.translation).normalize();
            direction.y = 0.0;
            transform.translation += direction * enemy.speed * time.delta_seconds();
            transform.look_at(
                Vec3::new(player_pos.x, transform.translation.y, player_pos.z),
                Vec3::Y,
            );

            if distance < 2.0 {
                println!("Enemy attacks for {} damage!", enemy.attack);
            }
        }
    }
}

fn combat(
    keyboard: Res<ButtonInput<KeyCode>>,
    mut player_query: Query<&mut Player>,
    mut enemy_query: Query<(&mut Enemy, &Transform), Without<Player>>,
    player_transform: Query<&Transform, With<Player>>,
) {
    if !keyboard.just_pressed(KeyCode::Space) {
        return;
    }

    let player_pos = player_transform.single().translation;
    let mut player = player_query.single_mut();

    for (mut enemy, transform) in enemy_query.iter_mut() {
        if !enemy.is_alive {
            continue;
        }

        let distance = transform.translation.distance(player_pos);
        if distance < 3.0 {
            enemy.health -= player.attack;
            println!("Enemy took {} damage! Health: {}/{}", 
                player.attack, enemy.health, enemy.max_health);

            if enemy.health <= 0 {
                enemy.is_alive = false;
                println!("Enemy defeated!");
            }
        }
    }
}
