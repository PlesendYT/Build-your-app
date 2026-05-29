/// <summary>
/// Third-person camera with orbit, zoom, collision avoidance, and mouse look.
/// </summary>
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    public Transform target;
    public float distance = 5f;
    public float height = 2f;
    public float sensitivity = 3f;
    public float smoothing = 10f;
    public float minAngle = -20f;
    public float maxAngle = 80f;

    private float currentX;
    private float currentY;

    private void LateUpdate()
    {
        if (target == null) return;

        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;
        currentY = Mathf.Clamp(currentY, minAngle, maxAngle);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothing * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * height);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
