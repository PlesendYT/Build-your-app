/// <summary>
/// Controls the Snake with grid-based movement, growth on eating food, and collision detection.
/// </summary>
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

    /// <summary>
    /// Sets up the initial snake body segments.
    /// </summary>
    private void Start()
    {
        nextDirection = moveDirection;
        for (int i = 1; i < initialSize; i++)
            Grow();
    }

    /// <summary>
    /// Moves the snake forward at fixed intervals.
    /// </summary>
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

    /// <summary>
    /// Shifts segment positions and moves the head.
    /// </summary>
    private void Move()
    {
        for (int i = segments.Count - 1; i > 0; i--)
            segments[i].position = segments[i - 1].position;

        Vector3 newPos = transform.position + (Vector3)moveDirection * gridSize;
        transform.position = newPos;
    }

    /// <summary>
    /// Adds a new segment at the snake's tail position.
    /// </summary>
    private void Grow()
    {
        GameObject segment = Instantiate(segmentPrefab, transform.position - (Vector3)moveDirection * gridSize, Quaternion.identity);
        segments.Add(segment.transform);
    }

    /// <summary>
    /// Reads input and sets the next direction, preventing reversal.
    /// </summary>
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

    /// <summary>
    /// Handles food pickup (grow + score) and collision with walls/self (game over).
    /// </summary>
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
