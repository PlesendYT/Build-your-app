using Godot;
using System;

public partial class Player : Node2D
{
    [Export] public float MoveDelay = 0.15f;
    public int Health = 10;
    public int MaxHealth = 10;
    public int Attack = 2;
    public int Defense = 1;
    public int Level = 1;
    public int XP = 0;

    private TileMap _tileMap;
    private Timer _turnTimer;
    private bool _canMove = true;
    private int _xpToNext = 10;

    public override void _Ready()
    {
        _tileMap = GetParent().GetNode<TileMap>("TileMap");
        _turnTimer = GetNode<Timer>("TurnTimer");
        _turnTimer.Timeout += () => _canMove = true;
        GlobalPosition = _tileMap.MapToLocal(new Vector2I(5, 5));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!_canMove) return;

        Vector2I direction = Vector2I.Zero;
        if (@event.IsActionPressed("move_up")) direction = new Vector2I(0, -1);
        else if (@event.IsActionPressed("move_down")) direction = new Vector2I(0, 1);
        else if (@event.IsActionPressed("move_left")) direction = new Vector2I(-1, 0);
        else if (@event.IsActionPressed("move_right")) direction = new Vector2I(1, 0);

        if (direction != Vector2I.Zero)
            TryMove(direction);
    }

    private async void TryMove(Vector2I direction)
    {
        Vector2I targetCell = _tileMap.LocalToMap(GlobalPosition) + direction;
        var tileData = _tileMap.GetCellTileData(0, targetCell);

        if (tileData != null && (bool)tileData.GetCustomData("walkable"))
        {
            _canMove = false;
            Vector2 targetPos = _tileMap.MapToLocal(targetCell);
            var tween = CreateTween();
            tween.TweenProperty(this, "global_position", targetPos, MoveDelay);
            await ToSignal(tween, "finished");
            AdvanceTurn();
        }
    }

    private void AdvanceTurn()
    {
        _canMove = false;
        GetTree().CallGroup("enemies", "TakeTurn");
        _turnTimer.Start(MoveDelay);
    }

    public void TakeDamage(int amount)
    {
        int actual = Mathf.Max(1, amount - Defense);
        Health -= actual;
        if (Health <= 0) Die();
    }

    public void AddXP(int amount)
    {
        XP += amount;
        while (XP >= _xpToNext)
        {
            XP -= _xpToNext;
            Level++;
            _xpToNext = (int)(_xpToNext * 1.5f);
            MaxHealth += 2;
            Health = MaxHealth;
            Attack++;
        }
    }

    private void Die()
    {
        GetTree().ReloadCurrentScene();
    }
}
