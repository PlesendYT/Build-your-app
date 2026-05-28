using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SnakeController : MonoBehaviour
{
    [Header("Movement")]
    public float moveInterval = 0.15f;
    public float gridSize = 1f;
    public GameObject segmentPrefab;
    public int initialSize = 3;

    private Vector2 moveDirection = Vector2.right;
    private Vector2 nextDirection;
    private float moveTimer;
    private List<Transform> segments = new List<Transform>();
    private bool isAlive = true;

    private void Start()
    {
        nextDirection = moveDirection;
        for (int i = 1; i < initialSize; i++)
            Grow();
    }

    private void Update()
    {
        if (!isAlive) return;

        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            moveDirection = nextDirection;
            Move();
            moveTimer = moveInterval;
        }
    }

    private void Move()
    {
        for (int i = segments.Count - 1; i > 0; i--)
            segments[i].position = segments[i - 1].position;

        Vector3 newPos = transform.position + (Vector3)moveDirection * gridSize;
        transform.position = newPos;
    }

    private void Grow()
    {
        GameObject segment = Instantiate(segmentPrefab, transform.position - (Vector3)moveDirection * gridSize, Quaternion.identity);
        segments.Add(segment.transform);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            nextDirection = input.x > 0 ? Vector2.right : Vector2.left;
        else if (Mathf.Abs(input.y) > 0)
            nextDirection = input.y > 0 ? Vector2.up : Vector2.down;

        if (nextDirection == -moveDirection)
            nextDirection = moveDirection;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
            GameManager.Instance.AddScore(10);
            Destroy(other.gameObject);
            FindObjectOfType<FoodSpawner>()?.SpawnFood();
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Segment"))
        {
            isAlive = false;
            GameManager.Instance.GameOver();
        }
    }
}
