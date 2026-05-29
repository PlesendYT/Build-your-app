// File: MainViewModel.cs
// ViewModel for the MVVM todo app. Manages the todo list with add, toggle, and remove operations.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppMVVM.Models;

namespace AppMVVM.ViewModels;

/// <summary>
/// ViewModel for the main page. Uses MVVM Community Toolkit for observable properties and relay commands.
/// Manages a collection of TodoItem objects with add, toggle-complete, and remove functionality.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string newTodoTitle = string.Empty;

    public ObservableCollection<TodoItem> Todos { get; } = new();

    /// <summary>
    /// Adds a new todo item with the current NewTodoTitle if it is not blank.
    /// </summary>
    [RelayCommand]
    private void AddTodo()
    {
        if (string.IsNullOrWhiteSpace(NewTodoTitle)) return;

        Todos.Add(new TodoItem
        {
            Id = Todos.Count + 1,
            Title = NewTodoTitle,
            IsCompleted = false
        });

        NewTodoTitle = string.Empty;
    }

    /// <summary>
    /// Toggles the IsCompleted state of the given todo item.
    /// </summary>
    [RelayCommand]
    private void ToggleTodo(TodoItem item)
    {
        if (item is not null)
        {
            item.IsCompleted = !item.IsCompleted;
            OnPropertyChanged(nameof(Todos));
        }
    }

    /// <summary>
    /// Removes the specified todo item from the list.
    /// </summary>
    [RelayCommand]
    private void RemoveTodo(TodoItem item)
    {
        if (item is not null)
            Todos.Remove(item);
    }
}
