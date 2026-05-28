using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float clampX = 8f;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(mousePos.x, -clampX, clampX);
        transform.position = pos;
    }
}
