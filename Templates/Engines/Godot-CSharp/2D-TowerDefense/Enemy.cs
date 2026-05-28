using Godot;
using System;

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
        Progress += Speed * (float)delta;

        if (ProgressRatio >= 1.0f)
            ReachedEnd();
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        _healthBar.Value = Health;
        if (Health <= 0) Die();
    }

    private void Die()
    {
        GetTree().CallGroup("game_manager", "EnemyKilled", GoldValue);
        QueueFree();
    }

    private void ReachedEnd()
    {
        GetTree().CallGroup("game_manager", "EnemyReachedEnd", this);
    }
}
