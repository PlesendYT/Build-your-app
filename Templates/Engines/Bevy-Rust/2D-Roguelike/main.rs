use bevy::prelude::*;
use std::collections::HashMap;

fn main() {
    App::new()
        .add_plugins(DefaultPlugins)
        .add_systems(Startup, setup)
        .add_systems(Update, (player_turn, enemy_turn, render_map))
        .run();
}

const MAP_WIDTH: i32 = 20;
const MAP_HEIGHT: i32 = 15;

#[derive(Clone, Copy, PartialEq)]
enum Tile {
    Wall,
    Floor,
    Corridor,
}

#[derive(Component)]
struct Player {
    health: i32,
    max_health: i32,
    attack: i32,
    defense: i32,
    level: i32,
    xp: i32,
    xp_to_next: i32,
}

#[derive(Component)]
struct Enemy {
    health: i32,
    attack: i32,
    defense: i32,
    xp_value: i32,
    is_alive: bool,
}

#[derive(Component)]
struct Position {
    x: i32,
    y: i32,
}

#[derive(Component)]
struct BlocksTile;

#[derive(Resource)]
struct Map {
    tiles: Vec<Vec<Tile>>,
    rooms: Vec<Rect>,
}

#[derive(Resource)]
struct TurnState {
    player_turn: bool,
    turn_count: u32,
}

#[derive(Clone, Copy)]
struct Rect {
    x: i32,
    y: i32,
    w: i32,
    h: i32,
}

impl Rect {
    fn center(&self) -> (i32, i32) {
        (self.x + self.w / 2, self.y + self.h / 2)
    }
}

fn setup(mut commands: Commands) {
    commands.spawn(Camera2dBundle::default());
    let map = generate_dungeon();
    
    let (player_x, player_y) = map.rooms[0].center();
    commands.spawn((
        SpriteBundle {
            sprite: Sprite {
                color: Color::BLUE,
                custom_size: Some(Vec2::new(16.0, 16.0)),
                ..default()
            },
            transform: Transform::from_xyz(
                player_x as f32 * 16.0,
                player_y as f32 * 16.0,
                0.0,
            ),
            ..default()
        },
        Player {
            health: 10,
            max_health: 10,
            attack: 2,
            defense: 1,
            level: 1,
            xp: 0,
            xp_to_next: 10,
        },
        Position { x: player_x, y: player_y },
        BlocksTile,
    ));

    for i in 0..3 {
        let room = &map.rooms[(i + 1) % map.rooms.len()];
        let (ex, ey) = room.center();
        commands.spawn((
            SpriteBundle {
                sprite: Sprite {
                    color: Color::RED,
                    custom_size: Some(Vec2::new(16.0, 16.0)),
                    ..default()
                },
                transform: Transform::from_xyz(ex as f32 * 16.0, ey as f32 * 16.0, 0.0),
                ..default()
            },
            Enemy {
                health: 5,
                attack: 2,
                defense: 0,
                xp_value: 5,
                is_alive: true,
            },
            Position { x: ex, y: ey },
            BlocksTile,
        ));
    }

    commands.insert_resource(map);
    commands.insert_resource(TurnState {
        player_turn: true,
        turn_count: 0,
    });
}

fn generate_dungeon() -> Map {
    let mut tiles = vec![vec![Tile::Wall; MAP_HEIGHT as usize]; MAP_WIDTH as usize];
    let mut rooms = Vec::new();

    let mut rng = fastrand::Rng::new();
    for _ in 0..8 {
        let w = rng.i32(4..8);
        let h = rng.i32(4..8);
        let x = rng.i32(1..(MAP_WIDTH - w - 1));
        let y = rng.i32(1..(MAP_HEIGHT - h - 1));
        let new_room = Rect { x, y, w, h };

        let mut overlaps = false;
        for room in &rooms {
            if new_room.x < room.x + room.w
                && new_room.x + new_room.w > room.x
                && new_room.y < room.y + room.h
                && new_room.y + new_room.h > room.y
            {
                overlaps = true;
                break;
            }
        }

        if !overlaps {
            for rx in x..(x + w) {
                for ry in y..(y + h) {
                    tiles[rx as usize][ry as usize] = Tile::Floor;
                }
            }
            rooms.push(new_room);
        }
    }

    for i in 0..rooms.len().saturating_sub(1) {
        let (x1, y1) = rooms[i].center();
        let (x2, y2) = rooms[i + 1].center();
        for x in x1.min(x2)..=x1.max(x2) {
            tiles[x as usize][y1 as usize] = Tile::Corridor;
        }
        for y in y1.min(y2)..=y1.max(y2) {
            tiles[x2 as usize][y as usize] = Tile::Corridor;
        }
    }

    Map { tiles, rooms }
}

fn player_turn(
    keyboard: Res<ButtonInput<KeyCode>>,
    turn_state: Res<TurnState>,
    mut player_query: Query<(&mut Position, &mut Transform), With<Player>>,
    mut enemy_query: Query<(&Position, &mut Enemy)>,
    map: Res<Map>,
) {
    if !turn_state.player_turn {
        return;
    }

    let (mut pos, mut transform) = player_query.single_mut();
    let mut dx = 0;
    let mut dy = 0;

    if keyboard.just_pressed(KeyCode::ArrowLeft) { dx = -1; }
    else if keyboard.just_pressed(KeyCode::ArrowRight) { dx = 1; }
    else if keyboard.just_pressed(KeyCode::ArrowUp) { dy = -1; }
    else if keyboard.just_pressed(KeyCode::ArrowDown) { dy = 1; }
    else { return; }

    let new_x = pos.x + dx;
    let new_y = pos.y + dy;

    if new_x < 0 || new_x >= MAP_WIDTH || new_y < 0 || new_y >= MAP_HEIGHT {
        return;
    }

    if map.tiles[new_x as usize][new_y as usize] == Tile::Wall {
        return;
    }

    let mut hit_enemy = false;
    for (epos, mut enemy) in enemy_query.iter_mut() {
        if epos.x == new_x && epos.y == new_y && enemy.is_alive {
            enemy.health -= 2;
            if enemy.health <= 0 {
                enemy.is_alive = false;
                println!("Enemy defeated!");
            }
            hit_enemy = true;
            break;
        }
    }

    if !hit_enemy {
        pos.x = new_x;
        pos.y = new_y;
        transform.translation.x = new_x as f32 * 16.0;
        transform.translation.y = new_y as f32 * 16.0;
    }
}

fn enemy_turn(
    mut turn_state: ResMut<TurnState>,
    mut player_query: Query<&mut Player>,
    mut enemy_query: Query<(&mut Position, &mut Enemy, &mut Transform), Without<Player>>,
    player_pos_query: Query<&Position, With<Player>>,
    map: Res<Map>,
) {
    if turn_state.player_turn {
        return;
    }

    let player_pos = player_pos_query.single();
    for (mut pos, mut enemy, mut transform) in enemy_query.iter_mut() {
        if !enemy.is_alive {
            continue;
        }

        let dx = (player_pos.x - pos.x).signum();
        let dy = (player_pos.y - pos.y).signum();
        let distance = (player_pos.x - pos.x).abs() + (player_pos.y - pos.y).abs();

        if distance <= 1 {
            for mut player in player_query.iter_mut() {
                player.health -= enemy.attack;
                println!("Player took damage! Health: {}", player.health);
                if player.health <= 0 {
                    println!("Game Over!");
                }
            }
        } else if distance < 8 {
            let new_x = pos.x + dx;
            let new_y = pos.y + dy;
            if new_x >= 0 && new_x < MAP_WIDTH && new_y >= 0 && new_y < MAP_HEIGHT {
                if map.tiles[new_x as usize][new_y as usize] != Tile::Wall {
                    pos.x = new_x;
                    pos.y = new_y;
                    transform.translation.x = new_x as f32 * 16.0;
                    transform.translation.y = new_y as f32 * 16.0;
                }
            }
        }
    }

    turn_state.player_turn = true;
    turn_state.turn_count += 1;
}

fn render_map(mut gizmos: Gizmos, map: Res<Map>) {
    for x in 0..MAP_WIDTH {
        for y in 0..MAP_HEIGHT {
            if map.tiles[x as usize][y as usize] == Tile::Wall {
                gizmos.rect_2d(
                    Vec2::new(x as f32 * 16.0, y as f32 * 16.0),
                    0.0,
                    Vec2::splat(16.0),
                    Color::DARK_GRAY,
                );
            }
        }
    }
}
