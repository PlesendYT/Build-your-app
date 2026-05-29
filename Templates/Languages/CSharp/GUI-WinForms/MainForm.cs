namespace GUIWinForms;

/// <summary>Main form with a counter, text input, and item list.</summary>
public class MainForm : Form
{
    private readonly TextBox _inputBox;
    private readonly Label _counterLabel;
    private readonly Button _clickButton;
    private readonly ListBox _itemList;
    private int _counter;

    /// <summary>Initialises the form and lays out all controls.</summary>
    public MainForm()
    {
        // Configure window
        Text = "WinForms Template";
        Size = new Size(500, 400);
        StartPosition = FormStartPosition.CenterScreen;

        // Table layout for vertical stacking
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            ColumnCount = 1,
            RowCount = 5,
        };

        // Title label
        var title = new Label
        {
            Text = "Hello, WinForms!",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = true,
        };

        // Input text box
        _inputBox = new TextBox
        {
            PlaceholderText = "Enter text...",
            Dock = DockStyle.Fill,
        };

        // Counter display
        _counterLabel = new Label
        {
            Text = "Count: 0",
            Font = new Font("Segoe UI", 14),
            AutoSize = true,
        };

        // Action button
        _clickButton = new Button
        {
            Text = "Click me",
            Dock = DockStyle.Fill,
            Height = 32,
        };
        _clickButton.Click += OnButtonClick;

        // Item list box
        _itemList = new ListBox { Dock = DockStyle.Fill };

        // Assemble layout
        layout.Controls.Add(title, 0, 0);
        layout.Controls.Add(_inputBox, 0, 1);
        layout.Controls.Add(_counterLabel, 0, 2);
        layout.Controls.Add(_clickButton, 0, 3);
        layout.Controls.Add(_itemList, 0, 4);

        Controls.Add(layout);
    }

    /// <summary>Handles button click: increments counter and adds input text to the list.</summary>
    private void OnButtonClick(object? sender, EventArgs e)
    {
        _counter++;
        _counterLabel.Text = $"Count: {_counter}";

        // Add non-empty input to the list box
        if (!string.IsNullOrWhiteSpace(_inputBox.Text))
        {
            _itemList.Items.Add(_inputBox.Text);
            _inputBox.Clear();
        }
    }
}
