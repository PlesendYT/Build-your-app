using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody3D
{
    [Export] public float WalkSpeed = 5.0f;
    [Export] public float SprintSpeed = 8.0f;
    [Export] public float JumpVelocity = 4.5f;
    [Export] public float Gravity = 9.8f;
    [Export] public float MouseSensitivity = 0.002f;

    private SpringArm3D _springArm;
    private Camera3D _camera;
    private Area3D _interactArea;

    public int Health = 100;
    public int MaxHealth = 100;
    public int Mana = 50;
    public int MaxMana = 50;
    public int Level = 1;
    public int XP = 0;
    public List<ItemData> Inventory = new();
    public List<QuestData> Quests = new();

    private int _xpToNext = 100;

    public override void _Ready()
    {
        _springArm = GetNode<SpringArm3D>("SpringArm3D");
        _camera = GetNode<Camera3D>("SpringArm3D/Camera3D");
        _interactArea = GetNode<Area3D>("InteractArea");
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            RotateY(-motion.Relative.X * MouseSensitivity);
            _springArm.RotateX(-motion.Relative.Y * MouseSensitivity);
            _springArm.Rotation = new Vector3(
                Mathf.Clamp(_springArm.Rotation.X, -1.0f, 0.5f),
                _springArm.Rotation.Y,
                _springArm.Rotation.Z
            );
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsOnFloor())
            Velocity = new Vector3(Velocity.X, Velocity.Y - Gravity * (float)delta, Velocity.Z);

        float speed = Input.IsActionPressed("sprint") ? SprintSpeed : WalkSpeed;
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, -inputDir.Y)).Normalized();

        Velocity = new Vector3(direction.X * speed, Velocity.Y, direction.Z * speed);

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            Velocity = new Vector3(Velocity.X, JumpVelocity, Velocity.Z);

        if (Input.IsActionJustPressed("interact"))
            Interact();

        MoveAndSlide();
    }

    private void Interact()
    {
        var overlaps = _interactArea.GetOverlappingAreas();
        foreach (var area in overlaps)
        {
            if (area.IsInGroup("interactable"))
            {
                area.Call("Interact", this);
                return;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0) Die();
    }

    public void AddXP(int amount)
    {
        XP += amount;
        while (XP >= _xpToNext)
        {
            XP -= _xpToNext;
            Level++;
            _xpToNext = (int)(_xpToNext * 1.5f);
            MaxHealth += 10;
            Health = MaxHealth;
            MaxMana += 5;
            Mana = MaxMana;
        }
    }

    public void AddItem(string itemName, int quantity = 1)
    {
        var existing = Inventory.Find(i => i.Name == itemName);
        if (existing != null)
            existing.Quantity += quantity;
        else
            Inventory.Add(new ItemData { Name = itemName, Quantity = quantity });
    }

    public bool RemoveItem(string itemName, int quantity = 1)
    {
        var item = Inventory.Find(i => i.Name == itemName);
        if (item == null || item.Quantity < quantity) return false;
        item.Quantity -= quantity;
        if (item.Quantity <= 0) Inventory.Remove(item);
        return true;
    }

    private void Die()
    {
        Health = MaxHealth;
        Position = Vector3.Zero;
    }

    public class ItemData
    {
        public string Name;
        public int Quantity;
    }

    public class QuestData
    {
        public string Id;
        public bool Completed;
        public int XpReward;
    }
}
