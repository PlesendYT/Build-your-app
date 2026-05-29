// File: GamePage.xaml.cs
// MAUI Game template: a simple coin-collection game rendered on a GraphicsView with pan-gesture controls.

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Game;

/// <summary>
/// Main game page. Hosts a GraphicsView with a GameDrawable, handles pan input, and runs a ~60 FPS game loop.
/// </summary>
public partial class GamePage : ContentPage
{
    private readonly GameDrawable _drawable;
    private readonly IDispatcherTimer _timer;

    public GamePage()
    {
        InitializeComponent();

        _drawable = new GameDrawable();
        GameCanvas.Drawable = _drawable;

        // Set up pan gesture for player movement
        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        GameCanvas.GestureRecognizers.Add(panGesture);

        // Start a game loop timer at ~60 FPS
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(16);
        _timer.Tick += (s, e) =>
        {
            _drawable.Update();
            GameCanvas.Invalidate();
        };
        _timer.Start();
    }

    /// <summary>
    /// Moves the player icon based on pan gesture delta.
    /// </summary>
    private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Running)
        {
            _drawable.MovePlayer((float)e.TotalX, (float)e.TotalY);
        }
    }
}

/// <summary>
/// Handles all game rendering and logic: player circle, coin spawning, collision detection, and scoring.
/// </summary>
public class GameDrawable : IDrawable
{
    private float _playerX = 200;
    private float _playerY = 300;
    private const float PlayerRadius = 25;

    private readonly List<Coin> _coins = new();

    private static readonly Random _rng = new();

    public int Score { get; private set; }

    public GameDrawable()
    {
        // Spawn initial coins
        for (int i = 0; i < 5; i++)
            SpawnCoin();
    }

    /// <summary>
    /// Moves the player, clamped within the canvas bounds.
    /// </summary>
    public void MovePlayer(float dx, float dy)
    {
        _playerX = Math.Clamp(_playerX + dx, PlayerRadius, 400 - PlayerRadius);
        _playerY = Math.Clamp(_playerY + dy, PlayerRadius, 600 - PlayerRadius);
    }

    /// <summary>
    /// Checks for collision between the player and each coin. On collision, the coin is collected and respawned.
    /// </summary>
    public void Update()
    {
        for (int i = _coins.Count - 1; i >= 0; i--)
        {
            var coin = _coins[i];
            var dx = _playerX - coin.X;
            var dy = _playerY - coin.Y;
            if (Math.Sqrt(dx * dx + dy * dy) < PlayerRadius + coin.Radius)
            {
                _coins.RemoveAt(i);
                Score++;
                SpawnCoin();
            }
        }
    }

    /// <summary>
    /// Spawns a coin at a random position within the canvas.
    /// </summary>
    private void SpawnCoin()
    {
        _coins.Add(new Coin
        {
            X = (float)(_rng.NextDouble() * 350 + 25),
            Y = (float)(_rng.NextDouble() * 550 + 25),
        });
    }

    /// <summary>
    /// Renders the background, score text, player circle, and all coin circles.
    /// </summary>
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Colors.White;
        canvas.FillRectangle(dirtyRect);

        canvas.FontColor = Colors.Black;
        canvas.FontSize = 20;
        canvas.DrawString($"Score: {Score}", 10, 10, HorizontalAlignment.Left);

        canvas.FillColor = Colors.Blue;
        canvas.FillCircle(_playerX, _playerY, PlayerRadius);

        canvas.FillColor = Colors.Gold;
        foreach (var coin in _coins)
            canvas.FillCircle(coin.X, coin.Y, coin.Radius);
    }
}

/// <summary>
/// Represents a collectible coin with position and a fixed radius.
/// </summary>
public class Coin
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Radius => 15;
}
