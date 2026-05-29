/// <summary>
/// Controls the paddle by tracking mouse position and clamping movement within horizontal bounds.
/// </summary>
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float clampX = 8f;
    private Camera mainCamera;

    /// <summary>
    /// Caches the main camera reference.
    /// </summary>
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Moves the paddle to follow the mouse's horizontal position, clamped to the play area.
    /// </summary>
    private void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(mousePos.x, -clampX, clampX);
        transform.position = pos;
    }
}
