/// <summary>
/// Handles aiming logic for the bubble shooter: draws a line from the shooter in the mouse direction and fires on click.
/// </summary>
using UnityEngine;

public class Aimer : MonoBehaviour
{
    public LineRenderer aimLine;
    public float maxDistance = 10f;
    public LayerMask obstacleMask;
    private Camera mainCamera;

    /// <summary>
    /// Caches the main camera reference.
    /// </summary>
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Updates the aim line direction based on mouse position and fires a bubble on left click.
    /// </summary>
    private void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 direction = (mousePos - transform.position).normalized;

        // Cast a ray to find the first obstacle and limit the aim line length
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, obstacleMask);
        float distance = hit.collider != null ? hit.distance : maxDistance;

        if (aimLine != null)
        {
            aimLine.positionCount = 2;
            aimLine.SetPosition(0, transform.position);
            aimLine.SetPosition(1, transform.position + (Vector3)direction * distance);
        }

        if (Input.GetMouseButtonDown(0))
        {
            BubbleShooterManager manager = FindObjectOfType<BubbleShooterManager>();
            if (manager != null)
                manager.ShootBubble(direction);
        }
    }
}
