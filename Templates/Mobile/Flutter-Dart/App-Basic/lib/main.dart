/// A basic Flutter counter app with Material 3 design.
import 'package:flutter/material.dart';

/// Application entry point.
void main() => runApp(const MyApp());

/// Root widget of the basic Flutter app.
class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'App Basic',
      theme: ThemeData(primarySwatch: Colors.blue, useMaterial3: true),
      home: const HomePage(),
    );
  }
}

/// The home page with a counter that increments on button press.
class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

/// State class managing the counter value and UI rendering.
class _HomePageState extends State<HomePage> {
  int _counter = 0;

  /// Increments the counter and triggers a UI rebuild.
  void _increment() => setState(() => _counter++);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('App Basic')),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Text('Hello, Build Your App!', style: TextStyle(fontSize: 24)),
            const SizedBox(height: 16),
            Text('Counter: $_counter', style: const TextStyle(fontSize: 20)),
          ],
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: _increment,
        child: const Icon(Icons.add),
      ),
    );
  }
}
