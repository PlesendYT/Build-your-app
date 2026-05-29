//! A drag-and-drop puzzle game where coloured pieces snap into matching drop zones.

use bevy::prelude::*;

/// Entry point: builds and runs the Bevy app.
fn main() {
    App::new()
        .add_plugins(DefaultPlugins)
        .add_systems(Startup, setup)
        .add_systems(Update, (drag_pieces, snap_check, puzzle_complete))
        .run();
}

/// A draggable puzzle piece that snaps to its matching drop zone.
#[derive(Component)]
struct PuzzlePiece {
    piece_type: String,
    start_pos: Vec3,
    is_placed: bool,
    is_dragging: bool,
}

/// A target zone that accepts one piece of a matching type.
#[derive(Component)]
struct DropZone {
    piece_type: String,
    is_occupied: bool,
}

/// Tracks overall puzzle completion state.
#[derive(Component)]
struct PuzzleManager {
    placed_count: u32,
    total_pieces: u32,
    completed: bool,
}

/// Offset between the mouse cursor and the piece's centre while dragging.
#[derive(Component)]
struct DragOffset(Vec3);

/// Spawns pieces, drop zones, camera, and the manager.
fn setup(mut commands: Commands, asset_server: Res<AssetServer>) {
    commands.spawn(Camera2dBundle::default());

    // Blue piece on the left.
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

    // Red piece on the left, lower down.
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

    // Blue drop zone on the right.
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

    // Red drop zone on the right, upper.
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

    // Puzzle manager (2 pieces total).
    commands.spawn(PuzzleManager {
        placed_count: 0,
        total_pieces: 2,
        completed: false,
    });
}

/// Handles mouse-based dragging of puzzle pieces.
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

        // Build an AABB for click detection.
        let piece_rect = Rect::from_center_size(
            transform.translation.truncate(),
            sprite.custom_size.unwrap_or(Vec2::splat(50.0)),
        );

        // Start dragging on click inside the piece.
        if mouse.just_pressed(MouseButton::Left) {
            if let Some(pos) = world_pos {
                if piece_rect.contains(pos) {
                    piece.is_dragging = true;
                    transform.translation.z = 10.0;
                }
            }
        }

        // Stop dragging on release.
        if mouse.just_released(MouseButton::Left) {
            if piece.is_dragging {
                piece.is_dragging = false;
                transform.translation.z = 0.0;
            }
        }

        // Follow the cursor while dragging.
        if piece.is_dragging {
            if let Some(pos) = world_pos {
                transform.translation = pos.extend(10.0);
            }
        }
    }
}

/// After a piece is released, checks whether it is close enough to snap to its matching zone.
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

/// Prints a completion message once all pieces are placed.
fn puzzle_complete(
    manager_query: Query<&PuzzleManager>,
) {
    let manager = manager_query.single();
    if manager.placed_count >= manager.total_pieces && !manager.completed {
        println!("Puzzle complete!");
    }
}
