using UnityEngine;
using UnityEngine.InputSystem;

public class DrawingInput : MonoBehaviour
{
    private DrawingCanvas canvas;
    private Camera mainCamera;
    private bool isDrawing;

    private void Awake()
    {
        canvas = FindObjectOfType<DrawingCanvas>();
        mainCamera = Camera.main;
    }

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
