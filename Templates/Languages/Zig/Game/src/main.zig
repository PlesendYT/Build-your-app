const std = @import("std");
const c = @cImport({
    @cInclude("raylib.h");
});

const ScreenWidth = 800;
const ScreenHeight = 600;
const PlayerSpeed = 5.0;
const BulletSpeed = 7.0;
const MaxBullets = 32;

const Player = struct {
    x: f32,
    y: f32,
    radius: f32,
    color: c.Color,

    fn init() Player {
        return Player{
            .x = @floatFromInt(ScreenWidth / 2),
            .y = @floatFromInt(ScreenHeight - 50),
            .radius = 20,
            .color = c.BLUE,
        };
    }

    fn update(self: *Player) void {
        if (c.IsKeyDown(c.KEY_LEFT) or c.IsKeyDown(c.KEY_A)) {
            self.x -= PlayerSpeed;
        }
        if (c.IsKeyDown(c.KEY_RIGHT) or c.IsKeyDown(c.KEY_D)) {
            self.x += PlayerSpeed;
        }
        if (c.IsKeyDown(c.KEY_UP) or c.IsKeyDown(c.KEY_W)) {
            self.y -= PlayerSpeed;
        }
        if (c.IsKeyDown(c.KEY_DOWN) or c.IsKeyDown(c.KEY_S)) {
            self.y += PlayerSpeed;
        }

        self.x = std.math.clamp(self.x, self.radius, @as(f32, @floatFromInt(ScreenWidth)) - self.radius);
        self.y = std.math.clamp(self.y, self.radius, @as(f32, @floatFromInt(ScreenHeight)) - self.radius);
    }

    fn draw(self: Player) void {
        c.DrawCircle(@intFromFloat(self.x), @intFromFloat(self.y), self.radius, self.color);
    }
};

const Bullet = struct {
    x: f32,
    y: f32,
    active: bool,

    fn init(x: f32, y: f32) Bullet {
        return Bullet{ .x = x, .y = y, .active = true };
    }

    fn update(self: *Bullet) void {
        if (self.active) {
            self.y -= BulletSpeed;
            if (self.y < 0) {
                self.active = false;
            }
        }
    }

    fn draw(self: Bullet) void {
        if (self.active) {
            c.DrawCircle(@intFromFloat(self.x), @intFromFloat(self.y), 5, c.YELLOW);
        }
    }
};

const Enemy = struct {
    x: f32,
    y: f32,
    radius: f32,
    active: bool,
    speed: f32,

    fn init(x: f32, y: f32) Enemy {
        return Enemy{
            .x = x,
            .y = y,
            .radius = 15,
            .active = true,
            .speed = 2.0,
        };
    }

    fn update(self: *Enemy) void {
        if (self.active) {
            self.y += self.speed;
            if (self.y > ScreenHeight + self.radius) {
                self.active = false;
            }
        }
    }

    fn draw(self: Enemy) void {
        if (self.active) {
            c.DrawCircle(@intFromFloat(self.x), @intFromFloat(self.y), self.radius, c.RED);
        }
    }
};

pub fn main() !void {
    c.InitWindow(ScreenWidth, ScreenHeight, "Zig Game");
    defer c.CloseWindow();

    c.SetTargetFPS(60);

    var player = Player.init();
    var bullets = [_]Bullet{undefined} ** MaxBullets;
    var enemies = [_]Enemy{undefined} ** 10;
    var enemy_count: usize = 0;
    var score: i32 = 0;
    var spawn_timer: f32 = 0;

    while (!c.WindowShouldClose()) {
        player.update();

        if (c.IsKeyPressed(c.KEY_SPACE)) {
            for (&bullets) |*b| {
                if (!b.active) {
                    b.* = Bullet.init(player.x, player.y);
                    break;
                }
            }
        }

        spawn_timer += c.GetFrameTime();
        if (spawn_timer > 1.5 and enemy_count < enemies.len) {
            const ex = @as(f32, @floatFromInt(c.GetRandomValue(20, ScreenWidth - 20)));
            enemies[enemy_count] = Enemy.init(ex, -15);
            enemy_count += 1;
            spawn_timer = 0;
        }

        for (&bullets) |*b| b.update();
        for (&enemies) |*e| e.update();

        for (&bullets) |*b| {
            if (!b.active) continue;
            for (&enemies) |*e| {
                if (!e.active) continue;
                const dx = b.x - e.x;
                const dy = b.y - e.y;
                const dist = std.math.sqrt(dx * dx + dy * dy);
                if (dist < e.radius + 5) {
                    b.active = false;
                    e.active = false;
                    score += 10;
                }
            }
        }

        for (&enemies) |*e| {
            if (!e.active) continue;
            const dx = player.x - e.x;
            const dy = player.y - e.y;
            const dist = std.math.sqrt(dx * dx + dy * dy);
            if (dist < player.radius + e.radius) {
                // Game over - reset
                player = Player.init();
                score = 0;
                enemy_count = 0;
                for (&enemies) |*en| en.* = .{ .x = 0, .y = 0, .radius = 15, .active = false, .speed = 2.0 };
                break;
            }
        }

        c.BeginDrawing();
        defer c.EndDrawing();

        c.ClearBackground(c.BLACK);

        player.draw();
        for (&bullets) |b| b.draw();
        for (&enemies) |e| e.draw();

        c.DrawText(&try std.fmt.allocPrint(std.heap.page_allocator, "Score: {d}", .{score}), 10, 10, 20, c.WHITE);
        c.DrawFPS(ScreenWidth - 100, 10);
    }
}
