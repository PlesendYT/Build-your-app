// File: Player.cs
// Player controller for a 2D platformer with movement, double-jump, animation, and damage handling.

using Godot;
using System;

/// <summary>
/// Controls the player character: horizontal movement, gravity, double-jump, animation state,
/// damage response, and coin collection.
/// </summary>
public partial class Player : CharacterBody2D
{
    [Export] public float Speed = 300.0f;
    [Export] public float JumpVelocity = -400.0f;
    [Export] public float Gravity = 980.0f;
    [Export] public float DoubleJumpVelocity = -350.0f;

    private AnimatedSprite2D _animatedSprite;
    private bool _hasDoubleJump = true;
    private int _jumpsRemaining = 2;
    private int _health = 3;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        // Apply gravity and horizontal input
        Vector2 velocity = Velocity;
        velocity.Y += Gravity * (float)delta;

        float direction = Input.GetAxis("move_left", "move_right");
        velocity.X = direction * Speed;

        // Handle jump and double-jump input
        if (Input.IsActionJustPressed("jump"))
        {
            if (IsOnFloor())
            {
                velocity.Y = JumpVelocity;
                _jumpsRemaining = 1;
            }
            else if (_jumpsRemaining > 0)
            {
                velocity.Y = DoubleJumpVelocity;
                _jumpsRemaining--;
            }
        }

        Velocity = velocity;
        MoveAndSlide();
        UpdateAnimation();
    }

    /// <summary>
    /// Updates the sprite animation based on movement state (idle, run, jump) and facing direction.
    /// </summary>
    private void UpdateAnimation()
    {
        if (!IsOnFloor())
            _animatedSprite.Play("jump");
        else if (Velocity.X != 0)
            _animatedSprite.Play("run");
        else
            _animatedSprite.Play("idle");

        if (Velocity.X > 0)
            _animatedSprite.FlipH = false;
        else if (Velocity.X < 0)
            _animatedSprite.FlipH = true;
    }

    /// <summary>
    /// Reduces health; reloads the scene when health reaches zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0)
            GetTree().ReloadCurrentScene();
    }

    public void CollectCoin()
    {
        GD.Print("Coin collected!");
    }
}
