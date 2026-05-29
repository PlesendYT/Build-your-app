// File: Vehicle.cs
// Racing vehicle with physics-based acceleration, steering, braking, lap tracking, and HUD speed display.

using Godot;
using System;

/// <summary>
/// A RigidBody3D-based racing vehicle. Supports throttle, brake, and steering via force/torque.
/// Tracks laps through checkpoints and displays speed in km/h on a Label3D.
/// </summary>
public partial class Vehicle : RigidBody3D
{
    [Export] public float EnginePower = 800.0f;
    [Export] public float BrakePower = 300.0f;
    [Export] public float SteeringAngle = 0.5f;
    [Export] public float MaxSpeed = 30.0f;

    private Label3D _speedLabel;
    public int CurrentLap = 0;
    public int TotalLaps = 3;
    public int CheckpointsPassed = 0;
    private int _totalCheckpoints;

    public override void _Ready()
    {
        _speedLabel = GetNode<Label3D>("SpeedLabel");
        _totalCheckpoints = GetTree().GetNodesInGroup("checkpoint").Count;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Read input
        float throttle = Input.IsActionPressed("accelerate") ? 1.0f : 0.0f;
        float brake = Input.IsActionPressed("brake") ? 1.0f : 0.0f;
        float steer = Input.GetAxis("steer_right", "steer_left");

        // Apply forward/backward force (capped at MaxSpeed)
        Vector3 acceleration = -GlobalTransform.Basis.Z * throttle * EnginePower;
        if (LinearVelocity.Length() < MaxSpeed)
            ApplyCentralForce(acceleration);

        if (brake > 0)
            ApplyCentralForce(GlobalTransform.Basis.Z * BrakePower);

        // Apply steering torque proportional to current speed
        if (Mathf.Abs(LinearVelocity.Length()) > 0.5f)
            ApplyTorque(new Vector3(0, steer * SteeringAngle * LinearVelocity.Length(), 0));

        // Update speed display
        float speedKmh = LinearVelocity.Length() * 3.6f;
        _speedLabel.Text = $"{speedKmh:F0} km/h";
    }

    /// <summary>
    /// Called when the vehicle passes a checkpoint. Advances lap progress if checkpoints are visited in order.
    /// </summary>
    public void OnCheckpointPassed(int checkpointId)
    {
        if (checkpointId == CheckpointsPassed)
        {
            CheckpointsPassed++;
            if (CheckpointsPassed >= _totalCheckpoints)
                CompleteLap();
        }
    }

    /// <summary>
    /// Completes the current lap. If all laps are done, the race ends.
    /// </summary>
    private void CompleteLap()
    {
        CurrentLap++;
        CheckpointsPassed = 0;
        if (CurrentLap >= TotalLaps)
            RaceFinished();
    }

    /// <summary>
    /// Stops physics processing and prints the final result.
    /// </summary>
    private void RaceFinished()
    {
        SetPhysicsProcess(false);
        GD.Print($"Race finished in {CurrentLap} laps!");
    }
}
