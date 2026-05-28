import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

void main() => runApp(const ProviderScope(child: MyApp()));

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'App MVVM',
      theme: ThemeData(primarySwatch: Colors.indigo, useMaterial3: true),
      home: const HomePage(),
    );
  }
}

class HomePage extends ConsumerWidget {
  const HomePage({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final todos = ref.watch(todoListProvider);
    return Scaffold(
      appBar: AppBar(title: const Text('MVVM App')),
      body: ListView.builder(
        itemCount: todos.length,
        itemBuilder: (_, i) => ListTile(
          title: Text(todos[i].title),
          leading: Checkbox(
            value: todos[i].completed,
            onChanged: (_) => ref.read(todoListProvider.notifier).toggle(i),
          ),
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () => ref.read(todoListProvider.notifier).add('New Todo ${todos.length + 1}'),
        child: const Icon(Icons.add),
      ),
    );
  }
}
