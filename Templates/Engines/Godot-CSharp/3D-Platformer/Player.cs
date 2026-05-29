// File: Player.cs
// 3D platformer player controller with smooth movement, double-jump, checkpoint system, and animation.

using Godot;
using System;

/// <summary>
/// Controls the 3D platformer character. Features acceleration-based movement relative to camera,
/// double-jump, coin collection, checkpoint saving/respawning, and sprite animation.
/// </summary>
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

        // Read input and transform into camera-relative movement
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Vector3 direction = (_cameraPivot.Transform.Basis * new Vector3(inputDir.X, 0, -inputDir.Y)).Normalized();

        // Smoothly accelerate/decelerate in the movement direction
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

        // Handle jump and double-jump
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

        // Reset jump count when landing
        if (IsOnFloor())
            _jumpCount = 0;

        UpdateAnimation();
    }

    /// <summary>
    /// Updates the AnimatedSprite3D based on grounded state and movement speed.
    /// </summary>
    private void UpdateAnimation()
    {
        if (!IsOnFloor())
            _animatedSprite.Play("jump");
        else if (Velocity.Length() > 0.5f)
            _animatedSprite.Play("run");
        else
            _animatedSprite.Play("idle");
    }

    /// <summary>
    /// Increments the coin counter when a coin is collected.
    /// </summary>
    public void CollectCoin()
    {
        CollectedCoins++;
    }

    /// <summary>
    /// Saves a checkpoint position for respawning.
    /// </summary>
    public void SetCheckpoint(Vector3 position)
    {
        CurrentCheckpoint = position;
    }

    /// <summary>
    /// Respawns the player at the last checkpoint with zero velocity.
    /// </summary>
    public void Die()
    {
        GlobalPosition = CurrentCheckpoint;
        Velocity = Vector3.Zero;
    }
}
