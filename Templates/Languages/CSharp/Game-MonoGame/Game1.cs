using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameMonoGame;

/// <summary>Main game class for the MonoGame collect-the-coins demo.</summary>
public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private SpriteFont? _font;
    private Vector2 _playerPos;
    private readonly List<Vector2> _coins = [];
    private int _score;
    private Texture2D? _playerTex;
    private Texture2D? _coinTex;
    private bool _won;

    /// <summary>Initialises graphics, content directory, and mouse visibility.</summary>
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    /// <summary>Sets initial player position and spawns coins at random locations.</summary>
    protected override void Initialize()
    {
        // Start player at centre of screen
        _playerPos = new Vector2(
            _graphics.PreferredBackBufferWidth / 2f,
            _graphics.PreferredBackBufferHeight / 2f);

        // Spawn 10 coins at random positions
        var rng = Random.Shared;
        for (int i = 0; i < 10; i++)
        {
            _coins.Add(new Vector2(
                rng.Next(50, _graphics.PreferredBackBufferWidth - 50),
                rng.Next(50, _graphics.PreferredBackBufferHeight - 50)));
        }

        base.Initialize();
    }

    /// <summary>Loads fonts and creates player/coin textures procedurally.</summary>
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("DefaultFont");

        // Create a 40x40 blue square for the player
        _playerTex = new Texture2D(GraphicsDevice, 40, 40);
        Color[] playerData = new Color[40 * 40];
        Array.Fill(playerData, Color.Blue);
        _playerTex.SetData(playerData);

        // Create a 20x20 gold square for coins
        _coinTex = new Texture2D(GraphicsDevice, 20, 20);
        Color[] coinData = new Color[20 * 20];
        Array.Fill(coinData, Color.Gold);
        _coinTex.SetData(coinData);
    }

    /// <summary>Handles input, movement, coin collection, and win condition each frame.</summary>
    protected override void Update(GameTime gameTime)
    {
        // Exit on Escape
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Calculate movement delta
        float speed = 300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
        var kState = Keyboard.GetState();

        // Arrow-key movement
        if (kState.IsKeyDown(Keys.Left)) _playerPos.X -= speed;
        if (kState.IsKeyDown(Keys.Right)) _playerPos.X += speed;
        if (kState.IsKeyDown(Keys.Up)) _playerPos.Y -= speed;
        if (kState.IsKeyDown(Keys.Down)) _playerPos.Y += speed;

        // Clamp player inside the screen bounds
        _playerPos.X = Math.Clamp(_playerPos.X, 0, _graphics.PreferredBackBufferWidth - 40);
        _playerPos.Y = Math.Clamp(_playerPos.Y, 0, _graphics.PreferredBackBufferHeight - 40);

        // Check coin collection (iterate backwards for safe removal)
        var playerRect = new Rectangle((int)_playerPos.X, (int)_playerPos.Y, 40, 40);
        for (int i = _coins.Count - 1; i >= 0; i--)
        {
            var coinRect = new Rectangle((int)_coins[i].X, (int)_coins[i].Y, 20, 20);
            if (playerRect.Intersects(coinRect))
            {
                _coins.RemoveAt(i);
                _score += 10;
            }
        }

        // Check win condition
        if (_coins.Count == 0) _won = true;

        base.Update(gameTime);
    }

    /// <summary>Renders the player, coins, score, and win message each frame.</summary>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch!.Begin();

        // Draw player
        _spriteBatch.Draw(_playerTex, _playerPos, Color.White);

        // Draw remaining coins
        foreach (var coin in _coins)
            _spriteBatch.Draw(_coinTex, coin, Color.White);

        // Draw score in top-left corner
        _spriteBatch.DrawString(_font!, $"Score: {_score}", new Vector2(10, 10), Color.White);

        // Draw centred "You Win!" message when all coins collected
        if (_won)
        {
            var text = "You Win!";
            var size = _font!.MeasureString(text);
            _spriteBatch.DrawString(_font, text,
                new Vector2(
                    _graphics.PreferredBackBufferWidth / 2f - size.X / 2,
                    _graphics.PreferredBackBufferHeight / 2f - size.Y / 2),
                Color.Green);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
