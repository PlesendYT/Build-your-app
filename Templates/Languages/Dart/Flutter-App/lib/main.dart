/// A Flutter app using Riverpod for state management and go_router for navigation.
/// Features a counter screen and a settings screen with dark mode toggle.
import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

/// Provider that exposes the current counter value and its notifier.
final counterProvider = StateNotifierProvider<CounterNotifier, int>((ref) {
  return CounterNotifier();
});

/// Manages the counter state with increment, decrement, and reset actions.
class CounterNotifier extends StateNotifier<int> {
  CounterNotifier() : super(0);

  void increment() => state++;
  void decrement() => state--;
  void reset() => state = 0;
}

/// Provider for the selected theme mode (system, light, or dark).
final themeProvider = StateProvider<ThemeMode>((ref) => ThemeMode.system);

/// The main home screen showing a counter with increment/decrement buttons and a settings link.
class HomeScreen extends ConsumerWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final count = ref.watch(counterProvider);

    return Scaffold(
      appBar: AppBar(title: const Text('Flutter App')),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Text('Zähler:', style: TextStyle(fontSize: 24)),
            Text('$count', style: const TextStyle(fontSize: 72, fontWeight: FontWeight.bold)),
            const SizedBox(height: 20),
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                FloatingActionButton(
                  onPressed: () => ref.read(counterProvider.notifier).decrement(),
                  child: const Icon(Icons.remove),
                ),
                const SizedBox(width: 20),
                FloatingActionButton(
                  onPressed: () => ref.read(counterProvider.notifier).increment(),
                  child: const Icon(Icons.add),
                ),
              ],
            ),
            const SizedBox(height: 10),
            TextButton(
              onPressed: () => ref.read(counterProvider.notifier).reset(),
              child: const Text('Reset'),
            ),
            const SizedBox(height: 20),
            ElevatedButton(
              onPressed: () => context.go('/settings'),
              child: const Text('Einstellungen'),
            ),
          ],
        ),
      ),
    );
  }
}

/// Settings screen with a dark mode toggle switch and a back button.
class SettingsScreen extends ConsumerWidget {
  const SettingsScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final themeMode = ref.watch(themeProvider);

    return Scaffold(
      appBar: AppBar(title: const Text('Einstellungen')),
      body: ListView(
        children: [
          SwitchListTile(
            title: const Text('Dunkelmodus'),
            subtitle: Text(themeMode == ThemeMode.dark ? 'Aktiv' : 'Inaktiv'),
            value: themeMode == ThemeMode.dark,
            onChanged: (value) {
              ref.read(themeProvider.notifier).state =
                  value ? ThemeMode.dark : ThemeMode.light;
            },
          ),
          ListTile(
            title: const Text('Zurück'),
            leading: const Icon(Icons.arrow_back),
            onTap: () => context.go('/'),
          ),
        ],
      ),
    );
  }
}

/// Provider that configures the GoRouter with routes for HomeScreen and SettingsScreen.
final routerProvider = Provider<GoRouter>((ref) {
  return GoRouter(
    routes: [
      GoRoute(path: '/', builder: (_, __) => const HomeScreen()),
      GoRoute(path: '/settings', builder: (_, __) => const SettingsScreen()),
    ],
  );
});

/// Root app widget that sets up MaterialApp.router with theme support.
class App extends ConsumerWidget {
  const App({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final router = ref.watch(routerProvider);
    final themeMode = ref.watch(themeProvider);

    return MaterialApp.router(
      title: 'Flutter App',
      theme: ThemeData(
        colorSchemeSeed: Colors.indigo,
        useMaterial3: true,
        brightness: Brightness.light,
      ),
      darkTheme: ThemeData(
        colorSchemeSeed: Colors.indigo,
        useMaterial3: true,
        brightness: Brightness.dark,
      ),
      themeMode: themeMode,
      routerConfig: router,
    );
  }
}

/// Application entry point. Wraps the app in a ProviderScope for Riverpod.
void main() {
  runApp(const ProviderScope(child: App()));
}
