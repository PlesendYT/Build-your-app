namespace GUIWinForms;

public class MainForm : Form
{
    private readonly TextBox _inputBox;
    private readonly Label _counterLabel;
    private readonly Button _clickButton;
    private readonly ListBox _itemList;
    private int _counter;

    public MainForm()
    {
        Text = "WinForms Template";
        Size = new Size(500, 400);
        StartPosition = FormStartPosition.CenterScreen;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            ColumnCount = 1,
            RowCount = 5,
        };

        var title = new Label
        {
            Text = "Hello, WinForms!",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = true,
        };

        _inputBox = new TextBox
        {
            PlaceholderText = "Enter text...",
            Dock = DockStyle.Fill,
        };

        _counterLabel = new Label
        {
            Text = "Count: 0",
            Font = new Font("Segoe UI", 14),
            AutoSize = true,
        };

        _clickButton = new Button
        {
            Text = "Click me",
            Dock = DockStyle.Fill,
            Height = 32,
        };
        _clickButton.Click += OnButtonClick;

        _itemList = new ListBox { Dock = DockStyle.Fill };

        layout.Controls.Add(title, 0, 0);
        layout.Controls.Add(_inputBox, 0, 1);
        layout.Controls.Add(_counterLabel, 0, 2);
        layout.Controls.Add(_clickButton, 0, 3);
        layout.Controls.Add(_itemList, 0, 4);

        Controls.Add(layout);
    }

    private void OnButtonClick(object? sender, EventArgs e)
    {
        _counter++;
        _counterLabel.Text = $"Count: {_counter}";

        if (!string.IsNullOrWhiteSpace(_inputBox.Text))
        {
            _itemList.Items.Add(_inputBox.Text);
            _inputBox.Clear();
        }
    }
}
