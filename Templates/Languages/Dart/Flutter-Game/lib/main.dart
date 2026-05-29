/// Top-level game file for a simple Flame-based shooter game with a player, enemies, and bullets.
import 'package:flutter/material.dart';
import 'package:flame/game.dart';
import 'package:flame/input.dart';
import 'package:flame/components.dart';

/// A circular player character controlled by arrow keys, with a reference to the parent game.
class Player extends CircleComponent with HasGameRef<MyGame> {
  Player()
      : super(
          radius: 20,
          position: Vector2(400, 500),
          anchor: Anchor.center,
        );

  final double _speed = 300;

  @override
  Future<void> onLoad() async {
    paint.color = const Color(0xFF3399FF);
  }

  @override
  void update(double dt) {
    final keys = gameRef.keyboard;
    if (keys.isKeyPressed(LogicalKeyboardKey.arrowLeft)) {
      position.x -= _speed * dt;
    }
    if (keys.isKeyPressed(LogicalKeyboardKey.arrowRight)) {
      position.x += _speed * dt;
    }
    if (keys.isKeyPressed(LogicalKeyboardKey.arrowUp)) {
      position.y -= _speed * dt;
    }
    if (keys.isKeyPressed(LogicalKeyboardKey.arrowDown)) {
      position.y += _speed * dt;
    }
    position.clamp(
      Vector2(radius, radius),
      Vector2(gameRef.size.x - radius, gameRef.size.y - radius),
    );
  }
}

/// A circular enemy that moves downward from the top of the screen.
class Enemy extends CircleComponent {
  Enemy(Vector2 position)
      : super(
          radius: 15,
          position: position,
          anchor: Anchor.center,
        );

  final double _speed = 100;

  @override
  Future<void> onLoad() async {
    paint.color = const Color(0xFFFF3333);
  }

  @override
  void update(double dt) {
    position.y += _speed * dt;
  }
}

/// A small bullet projectile that travels upward from the player's position.
class Bullet extends CircleComponent {
  Bullet(Vector2 position)
      : super(
          radius: 4,
          position: position,
          anchor: Anchor.center,
        );

  final double _speed = 400;

  @override
  Future<void> onLoad() async {
    paint.color = const Color(0xFFFFFF00);
  }

  @override
  void update(double dt) {
    position.y -= _speed * dt;
  }
}

/// The main game class that manages spawning enemies, handling collisions, and tracking the score.
class MyGame extends FlameGame with KeyboardEvents {
  late final Player _player;
  int score = 0;
  double _spawnTimer = 0;

  /// Initializes the player and adds it to the game world.
  @override
  Future<void> onLoad() async {
    _player = Player();
    add(_player);
  }

  @override
  void update(double dt) {
    super.update(dt);

    _spawnTimer += dt;
    if (_spawnTimer > 1.0) {
      _spawnTimer = 0;
      add(Enemy(Vector2(
        50 + (size.x - 100) * (DateTime.now().millisecondsSinceEpoch % 100) / 100,
        -30,
      )));
    }

    // Remove off-screen enemies and bullets
    for (final child in children.toList()) {
      if (child is Enemy && child.position.y > size.y + 30) {
        child.removeFromParent();
      }
      if (child is Bullet && child.position.y < -30) {
        child.removeFromParent();
      }
    }

    // Collision detection
    for (final enemy in children.whereType<Enemy>().toList()) {
      for (final bullet in children.whereType<Bullet>().toList()) {
        if (enemy.position.distanceTo(bullet.position) < enemy.radius + bullet.radius) {
          enemy.removeFromParent();
          bullet.removeFromParent();
          score += 10;
        }
      }
      if (enemy.position.distanceTo(_player.position) < enemy.radius + _player.radius) {
        _player.removeFromParent();
        add(TextComponent(
          text: 'GAME OVER - Score: $score',
          textRenderer: TextPaint(style: TextStyle(
            color: Colors.red,
            fontSize: 36,
            fontWeight: FontWeight.bold,
          )),
          anchor: Anchor.center,
          position: size / 2,
        ));
      }
    }
  }

  /// Handles keyboard input: fires a bullet from the player's position on spacebar press.
  @override
  KeyEventResult onKeyEvent(KeyEvent event, Set<LogicalKeyboardKey> keysPressed) {
    if (event is KeyDownEvent && event.logicalKey == LogicalKeyboardKey.space) {
      add(Bullet(Vector2(_player.position.x, _player.position.y - 30)));
    }
    return KeyEventResult.handled;
  }
}

/// Wraps the [MyGame] instance in a Flutter GameWidget for display.
class GameWidget extends StatelessWidget {
  const GameWidget({super.key});

  @override
  Widget build(BuildContext context) {
    return GameWidget(game: MyGame());
  }
}

/// Application entry point. Runs the game inside a MaterialApp.
void main() {
  runApp(const MaterialApp(
    title: 'Flutter Game',
    home: Scaffold(body: GameWidget()),
  ));
}
