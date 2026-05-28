using Godot;
using System;

public partial class DropZone : Area2D
{
    [Export] public string PieceType = "blue";
    public bool IsOccupied { get; set; } = false;
}
