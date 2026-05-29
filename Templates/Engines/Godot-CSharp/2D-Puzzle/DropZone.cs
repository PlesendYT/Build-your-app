// File: DropZone.cs
// Defines a target area where a puzzle piece of a matching type can be placed.

using Godot;
using System;

/// <summary>
/// A drop zone that accepts puzzle pieces of the matching PieceType. Tracks whether it is occupied.
/// </summary>
public partial class DropZone : Area2D
{
    [Export] public string PieceType = "blue";
    public bool IsOccupied { get; set; } = false;
}
