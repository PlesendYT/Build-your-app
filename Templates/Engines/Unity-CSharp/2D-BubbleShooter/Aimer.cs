using UnityEngine;

public class Aimer : MonoBehaviour
{
    public LineRenderer aimLine;
    public float maxDistance = 10f;
    public LayerMask obstacleMask;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 direction = (mousePos - transform.position).normalized;

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
