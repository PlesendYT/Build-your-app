using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Game;

public partial class GamePage : ContentPage
{
    private readonly GameDrawable _drawable;
    private readonly IDispatcherTimer _timer;

    public GamePage()
    {
        InitializeComponent();

        _drawable = new GameDrawable();
        GameCanvas.Drawable = _drawable;

        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        GameCanvas.GestureRecognizers.Add(panGesture);

        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
        _timer.Tick += (s, e) =>
        {
            _drawable.Update();
            GameCanvas.Invalidate();
        };
        _timer.Start();
    }

    private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Running)
        {
            _drawable.MovePlayer((float)e.TotalX, (float)e.TotalY);
        }
    }
}

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
        for (int i = 0; i < 5; i++)
            SpawnCoin();
    }

    public void MovePlayer(float dx, float dy)
    {
        _playerX = Math.Clamp(_playerX + dx, PlayerRadius, 400 - PlayerRadius);
        _playerY = Math.Clamp(_playerY + dy, PlayerRadius, 600 - PlayerRadius);
    }

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

    private void SpawnCoin()
    {
        _coins.Add(new Coin
        {
            X = (float)(_rng.NextDouble() * 350 + 25),
            Y = (float)(_rng.NextDouble() * 550 + 25),
        });
    }

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

public class Coin
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Radius => 15;
}
