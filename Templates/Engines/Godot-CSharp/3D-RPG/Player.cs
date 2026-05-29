// File: Player.cs
// 3D RPG player character with third-person camera, movement, interaction, inventory, and leveling.

using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Full-featured RPG player controller. Includes third-person camera via SpringArm3D,
/// WASD movement with sprint, jump, interaction with nearby objects, damage/health,
/// XP-based leveling with stat growth, and an inventory system (add/remove items).
/// </summary>
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

    // Third-person mouse look: rotate the player body horizontally and the spring arm vertically
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

        // Movement relative to the player's facing direction
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

    /// <summary>
    /// Finds the nearest overlapping area in the "interactable" group and calls its Interact method.
    /// </summary>
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

    /// <summary>
    /// Reduces health. Respawns the player at the origin on death.
    /// </summary>
    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0) Die();
    }

    /// <summary>
    /// Adds XP and handles level-up: increases max HP, Mana, and scales XP required for next level.
    /// </summary>
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

    /// <summary>
    /// Adds an item to the inventory, stacking with existing items of the same name.
    /// </summary>
    public void AddItem(string itemName, int quantity = 1)
    {
        var existing = Inventory.Find(i => i.Name == itemName);
        if (existing != null)
            existing.Quantity += quantity;
        else
            Inventory.Add(new ItemData { Name = itemName, Quantity = quantity });
    }

    /// <summary>
    /// Removes a specified quantity of an item from the inventory. Returns false if insufficient.
    /// </summary>
    public bool RemoveItem(string itemName, int quantity = 1)
    {
        var item = Inventory.Find(i => i.Name == itemName);
        if (item == null || item.Quantity < quantity) return false;
        item.Quantity -= quantity;
        if (item.Quantity <= 0) Inventory.Remove(item);
        return true;
    }

    /// <summary>
    /// Respawns the player at the world origin with full health.
    /// </summary>
    private void Die()
    {
        Health = MaxHealth;
        Position = Vector3.Zero;
    }

    /// <summary>
    /// Represents a stackable inventory item.
    /// </summary>
    public class ItemData
    {
        public string Name;
        public int Quantity;
    }

    /// <summary>
    /// Represents a tracked quest with completion state and XP reward.
    /// </summary>
    public class QuestData
    {
        public string Id;
        public bool Completed;
        public int XpReward;
    }
}
