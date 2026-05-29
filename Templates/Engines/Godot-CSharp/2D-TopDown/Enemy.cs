// File: Enemy.cs
// Top-down enemy AI that patrols waypoints and chases the player when detected.

using Godot;
using System;

/// <summary>
/// Enemy that alternates between patrolling a set of waypoints and chasing the player
/// when the player enters its detection area. Chasing speed is 1.5x patrol speed.
/// </summary>
public partial class Enemy : CharacterBody2D
{
    [Export] public float MoveSpeed = 80.0f;
    [Export] public int Damage = 1;

    private AnimatedSprite2D _animatedSprite;
    private Area2D _detectionArea;
    private Node2D _target = null;
    private bool _chasing = false;
    private Vector2[] _patrolPoints;
    private int _currentPointIndex = 0;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _detectionArea = GetNode<Area2D>("DetectionArea");
        _detectionArea.BodyEntered += OnBodyEntered;
        _detectionArea.BodyExited += OnBodyExited;

        // Set up a rectangular patrol route around the spawn position
        _patrolPoints = new Vector2[]
        {
            Position,
            Position + new Vector2(200, 0),
            Position + new Vector2(200, 200),
            Position + new Vector2(0, 200)
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_chasing && _target != null && IsInstanceValid(_target))
            ChaseTarget();
        else
            Patrol();

        MoveAndSlide();
        _animatedSprite.Play("walk");
    }

    /// <summary>
    /// Moves toward the current patrol waypoint; cycles to the next when close enough.
    /// </summary>
    private void Patrol()
    {
        Vector2 targetPos = _patrolPoints[_currentPointIndex];
        Velocity = (targetPos - Position).Normalized() * MoveSpeed;

        if (Position.DistanceTo(targetPos) < 10.0f)
            _currentPointIndex = (_currentPointIndex + 1) % _patrolPoints.Length;
    }

    /// <summary>
    /// Moves directly toward the target (player) at an increased speed.
    /// </summary>
    private void ChaseTarget()
    {
        Velocity = (_target.Position - Position).Normalized() * (MoveSpeed * 1.5f);
    }

    /// <summary>
    /// Starts chasing when the player enters the detection area.
    /// </summary>
    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            _chasing = true;
            _target = body;
        }
    }

    /// <summary>
    /// Stops chasing when the player leaves the detection area.
    /// </summary>
    private void OnBodyExited(Node2D body)
    {
        if (body == _target)
        {
            _chasing = false;
            _target = null;
        }
    }
}
