using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    [Export] public float PatrolSpeed = 100.0f;
    [Export] public float PatrolDistance = 200.0f;
    [Export] public int Damage = 1;

    private AnimatedSprite2D _animatedSprite;
    private Vector2 _startPosition;
    private bool _movingRight = true;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _startPosition = Position;
    }

    public override void _PhysicsProcess(double delta)
    {
        float distanceTraveled = Mathf.Abs(Position.X - _startPosition.X);
        if (distanceTraveled >= PatrolDistance)
            _movingRight = !_movingRight;

        Velocity = new Vector2(_movingRight ? PatrolSpeed : -PatrolSpeed, Velocity.Y);
        MoveAndSlide();
        _animatedSprite.Play("walk");
        _animatedSprite.FlipH = !_movingRight;
    }
}
