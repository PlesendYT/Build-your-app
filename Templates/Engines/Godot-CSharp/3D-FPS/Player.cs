using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Export] public float MouseSensitivity = 0.002f;
    [Export] public float WalkSpeed = 5.0f;
    [Export] public float SprintSpeed = 8.0f;
    [Export] public float JumpVelocity = 4.5f;
    [Export] public float Gravity = 9.8f;

    private Node3D _head;
    private Camera3D _camera;
    private Timer _gunTimer;
    private Timer _reloadTimer;

    public int Health = 100;
    public int MaxHealth = 100;
    public int Armor = 0;
    public int Ammo = 30;
    public int MaxAmmo = 30;
    public int ReserveAmmo = 90;

    private bool _isReloading = false;
    private bool _canShoot = true;

    public override void _Ready()
    {
        _head = GetNode<Node3D>("Head");
        _camera = GetNode<Camera3D>("Head/Camera3D");
        _gunTimer = GetNode<Timer>("GunTimer");
        _reloadTimer = GetNode<Timer>("ReloadTimer");
        _gunTimer.Timeout += () => _canShoot = true;
        _reloadTimer.Timeout += OnReloadFinished;

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            _head.RotateY(-motion.Relative.X * MouseSensitivity);
            _camera.RotateX(-motion.Relative.Y * MouseSensitivity);
            _camera.Rotation = new Vector3(
                Mathf.Clamp(_camera.Rotation.X, -1.4f, 1.4f),
                _camera.Rotation.Y,
                _camera.Rotation.Z
            );
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsOnFloor())
            Velocity = new Vector3(Velocity.X, Velocity.Y - Gravity * (float)delta, Velocity.Z);

        float speed = Input.IsActionPressed("sprint") ? SprintSpeed : WalkSpeed;
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Vector3 direction = (_head.Transform.Basis * new Vector3(inputDir.X, 0, -inputDir.Y)).Normalized();

        Velocity = new Vector3(
            direction.X * speed,
            Velocity.Y,
            direction.Z * speed
        );

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            Velocity = new Vector3(Velocity.X, JumpVelocity, Velocity.Z);

        MoveAndSlide();
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (Input.IsActionPressed("shoot") && _canShoot && !_isReloading && Ammo > 0)
            Shoot();
        else if (Input.IsActionJustPressed("reload") && Ammo < MaxAmmo && ReserveAmmo > 0)
            StartReload();
    }

    private void Shoot()
    {
        _canShoot = false;
        Ammo--;
        _gunTimer.Start(0.1f);

        var spaceState = GetWorld3D().DirectSpaceState;
        Vector2 crosshairPos = GetViewport().GetSize() / 2;
        Vector3 from = _camera.GlobalPosition;
        Vector3 to = _camera.ProjectPosition(crosshairPos, 1000.0f);
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        var result = spaceState.IntersectRay(query);

        if (result.Count > 0 && result["collider"].AsGodotObject() is Node3D collider)
        {
            if (collider.IsInGroup("enemy"))
                collider.Call("TakeDamage", 25);
        }
    }

    private void StartReload()
    {
        _isReloading = true;
        _reloadTimer.Start(2.0f);
    }

    private void OnReloadFinished()
    {
        int needed = MaxAmmo - Ammo;
        int available = Mathf.Min(needed, ReserveAmmo);
        Ammo += available;
        ReserveAmmo -= available;
        _isReloading = false;
    }

    public void TakeDamage(int amount)
    {
        if (Armor > 0)
        {
            int absorbed = Mathf.Min(Armor, (int)(amount * 0.5f));
            Armor -= absorbed;
            amount -= absorbed;
        }
        Health -= amount;
        if (Health <= 0) Die();
    }

    private void Die()
    {
        GetTree().ReloadCurrentScene();
    }
}
