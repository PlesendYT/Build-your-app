/// <summary>
/// Handles mouse input for drawing on the canvas by converting screen position to world coordinates.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class DrawingInput : MonoBehaviour
{
    private DrawingCanvas canvas;
    private Camera mainCamera;
    private bool isDrawing;

    /// <summary>
    /// Finds the DrawingCanvas and main camera on start.
    /// </summary>
    private void Awake()
    {
        canvas = FindObjectOfType<DrawingCanvas>();
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Draws on the canvas each frame while the left mouse button is held down.
    /// </summary>
    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            worldPos.z = 0f;
            canvas.DrawAt(worldPos);
        }
    }
}
