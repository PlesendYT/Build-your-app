// File: Enemy.cs
// Tower defense enemy that follows a predefined path. Has health, speed, and rewards gold when killed.

using Godot;
using System;

/// <summary>
/// An enemy that moves along a Path2D using PathFollow2D. When it reaches the end it damages the base;
/// when killed it rewards gold. A health bar displays remaining health.
/// </summary>
public partial class Enemy : PathFollow2D
{
    [Export] public int Health = 10;
    [Export] public float Speed = 100.0f;
    [Export] public int DamageToBase = 1;
    [Export] public int GoldValue = 10;

    private ProgressBar _healthBar;

    public override void _Ready()
    {
        _healthBar = GetNode<ProgressBar>("HealthBar");
        _healthBar.MaxValue = Health;
        _healthBar.Value = Health;
        AddToGroup("enemies");
    }

    public override void _PhysicsProcess(double delta)
    {
        // Move the enemy along the path
        Progress += Speed * (float)delta;

        if (ProgressRatio >= 1.0f)
            ReachedEnd();
    }

    /// <summary>
    /// Reduces health and updates the health bar. Calls Die if health reaches zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        Health -= amount;
        _healthBar.Value = Health;
        if (Health <= 0) Die();
    }

    /// <summary>
    /// Notifies the GameManager of the kill and removes this enemy from the scene.
    /// </summary>
    private void Die()
    {
        GetTree().CallGroup("game_manager", "EnemyKilled", GoldValue);
        QueueFree();
    }

    /// <summary>
    /// Notifies the GameManager that this enemy reached the end of the path.
    /// </summary>
    private void ReachedEnd()
    {
        GetTree().CallGroup("game_manager", "EnemyReachedEnd", this);
    }
}
