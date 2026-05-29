// File: Player.cs
// Turn-based roguelike player with tile-based movement, combat stats, XP/leveling, and turn management.

using Godot;
using System;

/// <summary>
/// Controls the roguelike player on a tile map. Moves one tile per turn with a cooldown timer,
/// supports damage/defense calculations, XP-based leveling, and turn-based enemy activation.
/// </summary>
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

        // Read directional input for tile movement
        Vector2I direction = Vector2I.Zero;
        if (@event.IsActionPressed("move_up")) direction = new Vector2I(0, -1);
        else if (@event.IsActionPressed("move_down")) direction = new Vector2I(0, 1);
        else if (@event.IsActionPressed("move_left")) direction = new Vector2I(-1, 0);
        else if (@event.IsActionPressed("move_right")) direction = new Vector2I(1, 0);

        if (direction != Vector2I.Zero)
            TryMove(direction);
    }

    /// <summary>
    /// Attempts to move the player one tile in the given direction. Only walkable tiles are allowed.
    /// Movement is animated via a tween; after movement the turn advances.
    /// </summary>
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

    /// <summary>
    /// Ends the current turn: signals enemies to act and starts the cooldown for the next player move.
    /// </summary>
    private void AdvanceTurn()
    {
        _canMove = false;
        GetTree().CallGroup("enemies", "TakeTurn");
        _turnTimer.Start(MoveDelay);
    }

    /// <summary>
    /// Reduces health by damage minus defense (minimum 1 damage).
    /// </summary>
    public void TakeDamage(int amount)
    {
        int actual = Mathf.Max(1, amount - Defense);
        Health -= actual;
        if (Health <= 0) Die();
    }

    /// <summary>
    /// Adds XP and handles level-up logic (increased stats, scaling XP requirement).
    /// </summary>
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

    /// <summary>
    /// Reloads the scene when the player dies.
    /// </summary>
    private void Die()
    {
        GetTree().ReloadCurrentScene();
    }
}
