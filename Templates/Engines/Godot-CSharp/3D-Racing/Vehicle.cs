using Godot;
using System;

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
        float throttle = Input.IsActionPressed("accelerate") ? 1.0f : 0.0f;
        float brake = Input.IsActionPressed("brake") ? 1.0f : 0.0f;
        float steer = Input.GetAxis("steer_right", "steer_left");

        Vector3 acceleration = -GlobalTransform.Basis.Z * throttle * EnginePower;
        if (LinearVelocity.Length() < MaxSpeed)
            ApplyCentralForce(acceleration);

        if (brake > 0)
            ApplyCentralForce(GlobalTransform.Basis.Z * BrakePower);

        if (Mathf.Abs(LinearVelocity.Length()) > 0.5f)
            ApplyTorque(new Vector3(0, steer * SteeringAngle * LinearVelocity.Length(), 0));

        float speedKmh = LinearVelocity.Length() * 3.6f;
        _speedLabel.Text = $"{speedKmh:F0} km/h";
    }

    public void OnCheckpointPassed(int checkpointId)
    {
        if (checkpointId == CheckpointsPassed)
        {
            CheckpointsPassed++;
            if (CheckpointsPassed >= _totalCheckpoints)
                CompleteLap();
        }
    }

    private void CompleteLap()
    {
        CurrentLap++;
        CheckpointsPassed = 0;
        if (CurrentLap >= TotalLaps)
            RaceFinished();
    }

    private void RaceFinished()
    {
        SetPhysicsProcess(false);
        GD.Print($"Race finished in {CurrentLap} laps!");
    }
}
