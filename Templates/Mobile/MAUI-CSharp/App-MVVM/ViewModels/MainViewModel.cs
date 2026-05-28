using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppMVVM.Models;

namespace AppMVVM.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string newTodoTitle = string.Empty;

    public ObservableCollection<TodoItem> Todos { get; } = new();

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

    [RelayCommand]
    private void ToggleTodo(TodoItem item)
    {
        if (item is not null)
        {
            item.IsCompleted = !item.IsCompleted;
            OnPropertyChanged(nameof(Todos));
        }
    }

    [RelayCommand]
    private void RemoveTodo(TodoItem item)
    {
        if (item is not null)
            Todos.Remove(item);
    }
}
