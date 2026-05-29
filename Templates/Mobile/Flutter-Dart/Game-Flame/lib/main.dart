/// Entry point for the Flame game app. Renders the game inside a MaterialApp.
import 'package:flame/game.dart';
import 'package:flutter/material.dart';
import 'game.dart';

/// Initializes and runs the app.
void main() => runApp(const MyApp());

/// The root widget of the game application.
class MyApp extends StatelessWidget {
  const MyApp({super.key});
  @override
  Widget build(BuildContext context) => const MaterialApp(home: GameWidgetPage());
}

/// Page that hosts the Flame [GameWidget] with the [MyFlameGame] instance.
class GameWidgetPage extends StatelessWidget {
  const GameWidgetPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: GameWidget(game: MyFlameGame()),
    );
  }
}
