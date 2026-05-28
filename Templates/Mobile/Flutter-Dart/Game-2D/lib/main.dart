import 'package:flutter/material.dart';
import 'dart:math';

void main() => runApp(const MyApp());

class MyApp extends StatelessWidget {
  const MyApp({super.key});
  @override
  Widget build(BuildContext context) => const MaterialApp(home: GamePage());
}

class GamePage extends StatefulWidget {
  const GamePage({super.key});
  @override
  State<GamePage> createState() => _GamePageState();
}

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
      setState(() => _update());
    });
    _ticker.start();
  }

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

class Coin {
  final double x, y;
  final double r = 12;
  bool collected = false;
  Coin({required this.x, required this.y});
}

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
