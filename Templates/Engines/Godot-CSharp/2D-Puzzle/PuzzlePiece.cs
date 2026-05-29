// File: PuzzlePiece.cs
// A draggable puzzle piece that snaps into drop zones. Tracks its start position and placement state.

using Godot;
using System;

/// <summary>
/// Represents a draggable puzzle piece. Players can click and drag it; on release it notifies the parent
/// to check if it was placed in a matching drop zone.
/// </summary>
public partial class PuzzlePiece : Area2D
{
    [Export] public string PieceType = "blue";
    public Vector2 StartPosition { get; set; }
    public bool IsPlaced { get; set; } = false;

    private bool _isDragging = false;
    private Vector2 _dragOffset;

    public override void _Ready()
    {
        StartPosition = Position;
    }

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                // Start dragging on left-click if not already placed
                if (mouseEvent.Pressed && !IsPlaced)
                {
                    _isDragging = true;
                    _dragOffset = GlobalPosition - GetGlobalMousePosition();
                    ZIndex = 10;
                }
                // Release and notify parent on mouse-up
                else if (!mouseEvent.Pressed && _isDragging)
                {
                    _isDragging = false;
                    ZIndex = 1;
                    GetParent().Call("OnPieceDropped", this);
                }
            }
        }
    }

    // Follow the mouse while dragging
    public override void _Process(double delta)
    {
        if (_isDragging)
            GlobalPosition = GetGlobalMousePosition() + _dragOffset;
    }
}
