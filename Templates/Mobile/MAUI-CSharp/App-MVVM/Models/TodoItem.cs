// File: TodoItem.cs
// Data model for a single todo item in the MVVM app.

namespace AppMVVM.Models;

/// <summary>
/// Represents a todo item with an ID, title, and completion status.
/// </summary>
public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
