import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../models/todo.dart';

class TodoListNotifier extends StateNotifier<List<Todo>> {
  TodoListNotifier() : super([]);

  void add(String title) => state = [...state, Todo(title: title)];

  void toggle(int index) {
    state = [
      for (int i = 0; i < state.length; i++)
        i == index ? state[i].copyWith(completed: !state[i].completed) : state[i],
    ];
  }

  void remove(int index) => state = [...state.take(index), ...state.skip(index + 1)];
}

final todoListProvider = StateNotifierProvider<TodoListNotifier, List<Todo>>((ref) {
  return TodoListNotifier();
});
