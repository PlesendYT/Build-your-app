using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Export] public float Speed = 8.0f;
    [Export] public float JumpVelocity = 8.0f;
    [Export] public float DoubleJumpVelocity = 7.0f;
    [Export] public float Gravity = 18.0f;
    [Export] public float Acceleration = 0.2f;

    private Node3D _cameraPivot;
    private Camera3D _camera;
    private AnimatedSprite3D _animatedSprite;

    private bool _hasDoubleJump = true;
    private int _jumpCount = 0;
    public int CollectedCoins = 0;
    public Vector3 CurrentCheckpoint;

    public override void _Ready()
    {
        _cameraPivot = GetNode<Node3D>("CameraPivot");
        _camera = GetNode<Camera3D>("CameraPivot/Camera3D");
        _animatedSprite = GetNode<AnimatedSprite3D>("AnimatedSprite3D");
        CurrentCheckpoint = GlobalPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;
        velocity.Y -= Gravity * (float)delta;

        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Vector3 direction = (_cameraPivot.Transform.Basis * new Vector3(inputDir.X, 0, -inputDir.Y)).Normalized();

        if (direction != Vector3.Zero)
        {
            velocity.X = Mathf.Lerp(velocity.X, direction.X * Speed, Acceleration);
            velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * Speed, Acceleration);
        }
        else
        {
            velocity.X = Mathf.Lerp(velocity.X, 0.0f, Acceleration);
            velocity.Z = Mathf.Lerp(velocity.Z, 0.0f, Acceleration);
        }

        if (Input.IsActionJustPressed("jump"))
        {
            if (IsOnFloor())
            {
                velocity.Y = JumpVelocity;
                _jumpCount = 1;
            }
            else if (_hasDoubleJump && _jumpCount < 2)
            {
                velocity.Y = DoubleJumpVelocity;
                _jumpCount++;
            }
        }

        Velocity = velocity;
        MoveAndSlide();

        if (IsOnFloor())
            _jumpCount = 0;

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (!IsOnFloor())
            _animatedSprite.Play("jump");
        else if (Velocity.Length() > 0.5f)
            _animatedSprite.Play("run");
        else
            _animatedSprite.Play("idle");
    }

    public void CollectCoin()
    {
        CollectedCoins++;
    }

    public void SetCheckpoint(Vector3 position)
    {
        CurrentCheckpoint = position;
    }

    public void Die()
    {
        GlobalPosition = CurrentCheckpoint;
        Velocity = Vector3.Zero;
    }
}
