using Godot;
using System;

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

    private void Patrol()
    {
        Vector2 targetPos = _patrolPoints[_currentPointIndex];
        Velocity = (targetPos - Position).Normalized() * MoveSpeed;

        if (Position.DistanceTo(targetPos) < 10.0f)
            _currentPointIndex = (_currentPointIndex + 1) % _patrolPoints.Length;
    }

    private void ChaseTarget()
    {
        Velocity = (_target.Position - Position).Normalized() * (MoveSpeed * 1.5f);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            _chasing = true;
            _target = body;
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body == _target)
        {
            _chasing = false;
            _target = null;
        }
    }
}
