// File: Player.cs
// Top-down player controller with 8-direction movement, dash ability, and sprite animation.

using Godot;
using System;

/// <summary>
/// Drivable player character for a top-down game. Supports WASD/arrow movement, a dash
/// with cooldown, and directional sprite animation for four cardinal orientations.
/// </summary>
public partial class Player : CharacterBody2D
{
    [Export] public float Speed = 200.0f;
    [Export] public float DashSpeed = 600.0f;
    [Export] public float DashDuration = 0.2f;
    [Export] public float DashCooldown = 1.0f;

    private AnimatedSprite2D _animatedSprite;
    private Timer _dashTimer;
    private Timer _cooldownTimer;
    private bool _isDashing = false;
    private bool _canDash = true;
    private Vector2 _facingDirection = Vector2.Down;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _dashTimer = GetNode<Timer>("DashTimer");
        _cooldownTimer = GetNode<Timer>("CooldownTimer");
        _dashTimer.Timeout += OnDashEnd;
        _cooldownTimer.Timeout += () => _canDash = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        // During dash, skip normal movement
        if (_isDashing)
        {
            MoveAndSlide();
            return;
        }

        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        if (direction != Vector2.Zero)
        {
            Velocity = direction * Speed;
            _facingDirection = direction;
            UpdateAnimation(direction);
        }
        else
        {
            Velocity = Vector2.Zero;
            _animatedSprite.Play("idle");
        }

        MoveAndSlide();
        HandleDash();
    }

    /// <summary>
    /// Chooses the correct walk animation (side, up, or down) and flips the sprite horizontally if facing left.
    /// </summary>
    private void UpdateAnimation(Vector2 direction)
    {
        _animatedSprite.Play("walk");
        if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
        {
            _animatedSprite.FlipH = direction.X < 0;
            _animatedSprite.Animation = "walk_side";
        }
        else if (direction.Y < 0)
            _animatedSprite.Animation = "walk_up";
        else
            _animatedSprite.Animation = "walk_down";
    }

    /// <summary>
    /// Triggers a dash in the current facing direction if the ability is available and not on cooldown.
    /// </summary>
    private void HandleDash()
    {
        if (Input.IsActionJustPressed("dash") && _canDash && !_isDashing)
        {
            _isDashing = true;
            _canDash = false;
            Velocity = _facingDirection * DashSpeed;
            _dashTimer.Start(DashDuration);
            _cooldownTimer.Start(DashCooldown);
        }
    }

    /// <summary>
    /// Resets velocity when the dash duration ends.
    /// </summary>
    private void OnDashEnd()
    {
        _isDashing = false;
        Velocity = Vector2.Zero;
    }
}
