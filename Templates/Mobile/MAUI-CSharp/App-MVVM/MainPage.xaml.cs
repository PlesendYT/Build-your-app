// File: MainPage.xaml.cs
// Main page for the MVVM template. Injects MainViewModel as the BindingContext.

using AppMVVM.ViewModels;

namespace AppMVVM;

/// <summary>
/// The primary page of the MVVM app. Receives a MainViewModel via constructor injection for data binding.
/// </summary>
public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
