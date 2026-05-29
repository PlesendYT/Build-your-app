/// Represents a single todo item with a title and completion status.
class Todo {
  final String title;
  final bool completed;

  const Todo({required this.title, this.completed = false});

  /// Returns a copy of this todo with optional new [title] and [completed] values.
  Todo copyWith({String? title, bool? completed}) {
    return Todo(
      title: title ?? this.title,
      completed: completed ?? this.completed,
    );
  }
}
