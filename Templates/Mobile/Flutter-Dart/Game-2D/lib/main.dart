/// A simple 2D coin-collection game using Flutter's CustomPaint and gesture detection.
import 'package:flutter/material.dart';
import 'dart:math';

/// Application entry point.
void main() => runApp(const MyApp());

/// Root widget of the 2D game app.
class MyApp extends StatelessWidget {
  const MyApp({super.key});
  @override
  Widget build(BuildContext context) => const MaterialApp(home: GamePage());
}

/// The main game page that manages the game loop, player position, and coin collection.
class GamePage extends StatefulWidget {
  const GamePage({super.key});
  @override
  State<GamePage> createState() => _GamePageState();
}

/// State class managing the game loop, player position, coin list, and score.
class _GamePageState extends State<GamePage> with SingleTickerProviderStateMixin {
  late final Ticker _ticker;
  double playerX = 200, playerY = 400;
  final double playerR = 25;
  final List<Coin> coins = [];
  int score = 0;
  final Random _rand = Random();

  @override
  void initState() {
    super.initState();
    for (int i = 0; i < 8; i++) {
      coins.add(Coin(
        x: _rand.nextDouble() * 380 + 10,
        y: _rand.nextDouble() * 380 + 10,
      ));
    }
    _ticker = createTicker((elapsed) {
      setState(() => _update()); // Trigger game state update on each tick
    });
    _ticker.start();
  }

  /// Checks each coin for collision with the player and marks collected ones.
  void _update() {
    for (final coin in coins) {
      if (!coin.collected && (playerX - coin.x).abs() < playerR + coin.r &&
          (playerY - coin.y).abs() < playerR + coin.r) {
        coin.collected = true;
        score++;
      }
    }
  }

  @override
  void dispose() {
    _ticker.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Score: $score')),
      body: GestureDetector(
        onPanUpdate: (d) {
          setState(() {
            playerX = (playerX + d.delta.dx).clamp(playerR, 400 - playerR);
            playerY = (playerY + d.delta.dy).clamp(playerR, 400 - playerR);
          });
        },
        child: CustomPaint(
          size: const Size(400, 400),
          painter: GamePainter(playerX, playerY, playerR, coins),
        ),
      ),
    );
  }
}

/// Represents a single collectible coin with a position and collected state.
class Coin {
  final double x, y;
  final double r = 12;
  bool collected = false;
  Coin({required this.x, required this.y});
}

/// Custom painter that renders the player circle and uncollected coins.
class GamePainter extends CustomPainter {
  final double px, py, pr;
  final List<Coin> coins;
  GamePainter(this.px, this.py, this.pr, this.coins);

  @override
  void paint(Canvas canvas, Size size) {
    canvas.drawColor(Colors.white, BlendMode.src);
    for (final coin in coins) {
      if (!coin.collected) {
        canvas.drawCircle(Offset(coin.x, coin.y), coin.r, Paint()..color = Colors.amber);
      }
    }
    canvas.drawCircle(Offset(px, py), pr, Paint()..color = Colors.blue);
  }

  @override
  bool shouldRepaint(covariant CustomPainter oldDelegate) => true;
}
