using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DesktopTool;

/// <summary>Main form displaying system processes with auto-refresh.</summary>
public class MainForm : Form
{
    private readonly ListView _processList;
    private readonly Button _refreshButton;
    private readonly Label _systemLabel;
    private readonly Timer _refreshTimer;

    /// <summary>Initialises the form, layout, and starts the refresh timer.</summary>
    public MainForm()
    {
        // Configure window
        Text = "Desktop Tool";
        Size = new Size(700, 500);
        StartPosition = FormStartPosition.CenterScreen;

        // Table layout for vertical stacking
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            ColumnCount = 1,
            RowCount = 4,
        };

        // System info label at top
        _systemLabel = new Label
        {
            Text = $"OS: {Environment.OSVersion} | .NET: {Environment.Version}",
            Font = new Font("Segoe UI", 10),
            Dock = DockStyle.Fill,
            AutoSize = true,
        };

        // Refresh button
        _refreshButton = new Button
        {
            Text = "Refresh",
            Width = 100,
            Height = 30,
        };
        _refreshButton.Click += async (_, _) => await RefreshProcesses();

        // Process list view with three columns
        _processList = new ListView
        {
            View = View.Details,
            Dock = DockStyle.Fill,
            FullRowSelect = true,
            Columns =
            {
                new ColumnHeader { Text = "Process", Width = 250 },
                new ColumnHeader { Text = "PID", Width = 100 },
                new ColumnHeader { Text = "Memory (MB)", Width = 120 },
            },
        };

        // Assemble layout
        layout.Controls.Add(_systemLabel, 0, 0);
        layout.Controls.Add(_refreshButton, 0, 1);
        layout.Controls.Add(_processList, 0, 2);
        Controls.Add(layout);

        // Auto-refresh every 5 seconds
        _refreshTimer = new Timer { Interval = 5000 };
        _refreshTimer.Tick += async (_, _) => await RefreshProcesses();
        _refreshTimer.Start();

        // Initial load
        _ = RefreshProcesses();
    }

    /// <summary>Populates the list view with the top 50 processes by memory usage.</summary>
    private async Task RefreshProcesses()
    {
        _processList.BeginUpdate();
        _processList.Items.Clear();

        // Fetch processes on background thread
        var processes = await Task.Run(() =>
            Process.GetProcesses()
                .OrderByDescending(p => p.WorkingSet64)
                .Take(50)
                .Select(p => new
                {
                    p.ProcessName,
                    p.Id,
                    Memory = p.WorkingSet64 / (1024.0 * 1024.0)
                })
                .ToList());

        // Add each process to the list view
        foreach (var p in processes)
        {
            _processList.Items.Add(new ListViewItem([
                p.ProcessName, p.Id.ToString(), $"{p.Memory:F1}"
            ]));
        }

        _processList.EndUpdate();
    }

    /// <summary>Stops the refresh timer when the form is closed.</summary>
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _refreshTimer?.Stop();
        base.OnFormClosed(e);
    }
}
