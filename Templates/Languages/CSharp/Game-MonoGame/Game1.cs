using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameMonoGame;

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

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _playerPos = new Vector2(
            _graphics.PreferredBackBufferWidth / 2f,
            _graphics.PreferredBackBufferHeight / 2f);

        var rng = Random.Shared;
        for (int i = 0; i < 10; i++)
        {
            _coins.Add(new Vector2(
                rng.Next(50, _graphics.PreferredBackBufferWidth - 50),
                rng.Next(50, _graphics.PreferredBackBufferHeight - 50)));
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("DefaultFont");

        _playerTex = new Texture2D(GraphicsDevice, 40, 40);
        Color[] playerData = new Color[40 * 40];
        Array.Fill(playerData, Color.Blue);
        _playerTex.SetData(playerData);

        _coinTex = new Texture2D(GraphicsDevice, 20, 20);
        Color[] coinData = new Color[20 * 20];
        Array.Fill(coinData, Color.Gold);
        _coinTex.SetData(coinData);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        float speed = 300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
        var kState = Keyboard.GetState();

        if (kState.IsKeyDown(Keys.Left)) _playerPos.X -= speed;
        if (kState.IsKeyDown(Keys.Right)) _playerPos.X += speed;
        if (kState.IsKeyDown(Keys.Up)) _playerPos.Y -= speed;
        if (kState.IsKeyDown(Keys.Down)) _playerPos.Y += speed;

        _playerPos.X = Math.Clamp(_playerPos.X, 0, _graphics.PreferredBackBufferWidth - 40);
        _playerPos.Y = Math.Clamp(_playerPos.Y, 0, _graphics.PreferredBackBufferHeight - 40);

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

        if (_coins.Count == 0) _won = true;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch!.Begin();

        _spriteBatch.Draw(_playerTex, _playerPos, Color.White);

        foreach (var coin in _coins)
            _spriteBatch.Draw(_coinTex, coin, Color.White);

        _spriteBatch.DrawString(_font!, $"Score: {_score}", new Vector2(10, 10), Color.White);

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
