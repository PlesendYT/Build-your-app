using Godot;
using System;

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
                if (mouseEvent.Pressed && !IsPlaced)
                {
                    _isDragging = true;
                    _dragOffset = GlobalPosition - GetGlobalMousePosition();
                    ZIndex = 10;
                }
                else if (!mouseEvent.Pressed && _isDragging)
                {
                    _isDragging = false;
                    ZIndex = 1;
                    GetParent().Call("OnPieceDropped", this);
                }
            }
        }
    }

    public override void _Process(double delta)
    {
        if (_isDragging)
            GlobalPosition = GetGlobalMousePosition() + _dragOffset;
    }
}
