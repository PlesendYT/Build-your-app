/// Riverpod state notifier and provider for managing a list of Todo items.
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../models/todo.dart';

/// Manages the todo list state with add, toggle, and remove operations.
class TodoListNotifier extends StateNotifier<List<Todo>> {
  TodoListNotifier() : super([]);

  /// Adds a new todo with the given [title] to the list.
  void add(String title) => state = [...state, Todo(title: title)];

  /// Toggles the completed status of the todo at [index].
  void toggle(int index) {
    state = [
      for (int i = 0; i < state.length; i++)
        i == index ? state[i].copyWith(completed: !state[i].completed) : state[i],
    ];
  }

  /// Removes the todo at [index] from the list.
  void remove(int index) => state = [...state.take(index), ...state.skip(index + 1)];
}

/// Provider that exposes the [TodoListNotifier] and the current list of todos.
final todoListProvider = StateNotifierProvider<TodoListNotifier, List<Todo>>((ref) {
  return TodoListNotifier();
});
