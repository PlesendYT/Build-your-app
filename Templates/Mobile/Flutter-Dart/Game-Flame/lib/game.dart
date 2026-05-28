import 'dart:math';
import 'package:flame/collisions.dart';
import 'package:flame/components.dart';
import 'package:flame/game.dart';
import 'package:flutter/material.dart';

class MyFlameGame extends FlameGame with HasCollisionDetection {
  late final Player _player;
  int score = 0;

  @override
  Future<void> onLoad() async {
    _player = Player();
    add(_player);

    add(
      _spawnTimer = TimerComponent(
        period: 2,
        repeat: true,
        onTick: () {
          final coin = Coin(position: Vector2(
            Random().nextDouble() * size.x,
            Random().nextDouble() * size.y,
          ));
          add(coin);
        },
      ),
    );
  }

  late final TimerComponent _spawnTimer;

  void incrementScore() {
    score++;
  }
}

class Player extends CircleComponent with CollisionCallbacks {
  Player()
      : super(
          radius: 20,
          position: Vector2(200, 400),
          anchor: Anchor.center,
        ) {
    add(CircleHitbox());
  }

  @override
  Future<void> onLoad() async {
    super.onLoad();
    paint = Paint()..color = Colors.blue;
  }

  @override
  void update(double dt) {
    super.update(dt);
    // Simple keyboard-less movement placeholder
    // In a real game, use KeyboardHandler or touch input
  }
}

class Coin extends CircleComponent with CollisionCallbacks {
  Coin({required Vector2 position})
      : super(
          radius: 12,
          position: position,
          anchor: Anchor.center,
        ) {
    add(CircleHitbox());
  }

  @override
  Future<void> onLoad() async {
    super.onLoad();
    paint = Paint()..color = Colors.amber;
  }

  @override
  void onCollision(Set<Vector2> intersectionPoints, PositionComponent other) {
    super.onCollision(intersectionPoints, other);
    if (other is Player) {
      removeFromParent();
      (findParent() as MyFlameGame?)?.incrementScore();
    }
  }
}
