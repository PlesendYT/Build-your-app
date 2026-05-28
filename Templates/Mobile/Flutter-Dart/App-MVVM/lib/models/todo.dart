class Todo {
  final String title;
  final bool completed;

  const Todo({required this.title, this.completed = false});

  Todo copyWith({String? title, bool? completed}) {
    return Todo(
      title: title ?? this.title,
      completed: completed ?? this.completed,
    );
  }
}
