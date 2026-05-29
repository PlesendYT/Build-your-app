/// <summary>
/// Platform that moves between two points, carrying the player on top.
/// </summary>
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 moveDirection = Vector3.right;
    public float moveDistance = 5f;
    public float moveSpeed = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private int direction = 1;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + moveDirection.normalized * moveDistance;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            direction == 1 ? targetPosition : startPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, direction == 1 ? targetPosition : startPosition) < 0.1f)
        {
            direction *= -1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 start = Application.isPlaying ? startPosition : transform.position;
        Gizmos.DrawLine(start, start + moveDirection.normalized * moveDistance);
        Gizmos.DrawWireSphere(start + moveDirection.normalized * moveDistance, 0.3f);
    }
}
