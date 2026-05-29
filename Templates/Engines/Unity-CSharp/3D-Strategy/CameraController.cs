/// <summary>
/// RTS-style camera with pan, zoom, and bounds clamping.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    public float panSpeed = 20f;
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 50f;
    public float rotationSpeed = 100f;
    public Vector2 panBounds = new Vector2(50f, 50f);

    private Vector2 moveInput;
    private float zoomInput;

    private void Update()
    {
        HandlePan();
        HandleZoom();
    }

    private void HandlePan()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * panSpeed * Time.deltaTime;
        Vector3 newPos = transform.position + move;
        newPos.x = Mathf.Clamp(newPos.x, -panBounds.x, panBounds.x);
        newPos.z = Mathf.Clamp(newPos.z, -panBounds.y, panBounds.y);
        transform.position = newPos;
    }

    private void HandleZoom()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y - zoomInput * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
        transform.position = pos;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        zoomInput = context.ReadValue<float>();
    }
}
