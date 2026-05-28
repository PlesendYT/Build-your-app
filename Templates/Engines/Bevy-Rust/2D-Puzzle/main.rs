use bevy::prelude::*;

fn main() {
    App::new()
        .add_plugins(DefaultPlugins)
        .add_systems(Startup, setup)
        .add_systems(Update, (drag_pieces, snap_check, puzzle_complete))
        .run();
}

#[derive(Component)]
struct PuzzlePiece {
    piece_type: String,
    start_pos: Vec3,
    is_placed: bool,
    is_dragging: bool,
}

#[derive(Component)]
struct DropZone {
    piece_type: String,
    is_occupied: bool,
}

#[derive(Component)]
struct PuzzleManager {
    placed_count: u32,
    total_pieces: u32,
    completed: bool,
}

#[derive(Component)]
struct DragOffset(Vec3);

fn setup(mut commands: Commands, asset_server: Res<AssetServer>) {
    commands.spawn(Camera2dBundle::default());

    commands.spawn((
        SpriteBundle {
            sprite: Sprite {
                color: Color::BLUE,
                custom_size: Some(Vec2::new(50.0, 50.0)),
                ..default()
            },
            transform: Transform::from_xyz(-100.0, 0.0, 0.0),
            ..default()
        },
        PuzzlePiece {
            piece_type: "blue".to_string(),
            start_pos: Vec3::new(-100.0, 0.0, 0.0),
            is_placed: false,
            is_dragging: false,
        },
    ));

    commands.spawn((
        SpriteBundle {
            sprite: Sprite {
                color: Color::RED,
                custom_size: Some(Vec2::new(50.0, 50.0)),
                ..default()
            },
            transform: Transform::from_xyz(-100.0, -100.0, 0.0),
            ..default()
        },
        PuzzlePiece {
            piece_type: "red".to_string(),
            start_pos: Vec3::new(-100.0, -100.0, 0.0),
            is_placed: false,
            is_dragging: false,
        },
    ));

    commands.spawn((
        SpriteBundle {
            sprite: Sprite {
                color: Color::WHITE.with_a(0.3),
                custom_size: Some(Vec2::new(60.0, 60.0)),
                ..default()
            },
            transform: Transform::from_xyz(100.0, 0.0, 0.0),
            ..default()
        },
        DropZone {
            piece_type: "blue".to_string(),
            is_occupied: false,
        },
    ));

    commands.spawn((
        SpriteBundle {
            sprite: Sprite {
                color: Color::WHITE.with_a(0.3),
                custom_size: Some(Vec2::new(60.0, 60.0)),
                ..default()
            },
            transform: Transform::from_xyz(100.0, 100.0, 0.0),
            ..default()
        },
        DropZone {
            piece_type: "red".to_string(),
            is_occupied: false,
        },
    ));

    commands.spawn(PuzzleManager {
        placed_count: 0,
        total_pieces: 2,
        completed: false,
    });
}

fn drag_pieces(
    mouse: Res<ButtonInput<MouseButton>>,
    windows: Query<&Window>,
    cameras: Query<(&Camera, &GlobalTransform)>,
    mut query: Query<(&mut PuzzlePiece, &mut Transform, &mut Sprite)>,
) {
    let window = windows.single();
    let (camera, camera_transform) = cameras.single();
    let Ok(mouse_pos) = window.cursor_position() else { return };
    let world_pos = camera.viewport_to_world_2d(camera_transform, mouse_pos);

    for (mut piece, mut transform, mut sprite) in query.iter_mut() {
        if piece.is_placed {
            continue;
        }

        let piece_rect = Rect::from_center_size(
            transform.translation.truncate(),
            sprite.custom_size.unwrap_or(Vec2::splat(50.0)),
        );

        if mouse.just_pressed(MouseButton::Left) {
            if let Some(pos) = world_pos {
                if piece_rect.contains(pos) {
                    piece.is_dragging = true;
                    transform.translation.z = 10.0;
                }
            }
        }

        if mouse.just_released(MouseButton::Left) {
            if piece.is_dragging {
                piece.is_dragging = false;
                transform.translation.z = 0.0;
            }
        }

        if piece.is_dragging {
            if let Some(pos) = world_pos {
                transform.translation = pos.extend(10.0);
            }
        }
    }
}

fn snap_check(
    mut piece_query: Query<(&mut PuzzlePiece, &Transform)>,
    zone_query: Query<(&DropZone, &Transform, Entity)>,
    mut manager_query: Query<&mut PuzzleManager>,
) {
    let mut manager = manager_query.single_mut();
    let snap_distance = 30.0;

    for (mut piece, piece_transform) in piece_query.iter_mut() {
        if piece.is_placed || piece.is_dragging {
            continue;
        }

        for (zone, zone_transform, _) in zone_query.iter() {
            if zone.is_occupied || zone.piece_type != piece.piece_type {
                continue;
            }

            let distance = piece_transform.translation.distance(zone_transform.translation);
            if distance < snap_distance {
                piece.is_placed = true;
                manager.placed_count += 1;
                println!("Piece placed! {}/{}", manager.placed_count, manager.total_pieces);
            }
        }
    }
}

fn puzzle_complete(
    manager_query: Query<&PuzzleManager>,
) {
    let manager = manager_query.single();
    if manager.placed_count >= manager.total_pieces && !manager.completed {
        println!("Puzzle complete!");
    }
}
