using UnityEngine;
using UnityEngine.InputSystem;

public class TetrominoController : MonoBehaviour
{
    [Header("Movement")]
    public float fallInterval = 1f;
    private float fallTimer;
    private TetrisBoard board;
    private bool isActive = true;

    private void Awake()
    {
        board = FindObjectOfType<TetrisBoard>();
    }

    private void Start()
    {
        fallTimer = fallInterval;
    }

    private void Update()
    {
        if (!isActive) return;

        fallTimer -= Time.deltaTime;
        if (fallTimer <= 0f)
        {
            Move(Vector2Int.down);
            fallTimer = fallInterval;
        }
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed) Move(Vector2Int.left);
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed) Move(Vector2Int.right);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.performed) Rotate();
    }

    public void OnSoftDrop(InputAction.CallbackContext context)
    {
        if (context.performed) Move(Vector2Int.down);
    }

    public void OnHardDrop(InputAction.CallbackContext context)
    {
        if (context.performed) HardDrop();
    }

    private void Move(Vector2Int direction)
    {
        if (!isActive) return;

        Vector3 newPos = transform.position + new Vector3(direction.x, direction.y, 0f);
        if (IsValidPosition(newPos))
        {
            transform.position = newPos;
        }
        else if (direction == Vector2Int.down)
        {
            isActive = false;
            board.PlacePiece(transform);
        }
    }

    private void Rotate()
    {
        if (!isActive) return;
        transform.Rotate(0f, 0f, -90f);
        if (!IsValidPosition(transform.position))
            transform.Rotate(0f, 0f, 90f);
    }

    private void HardDrop()
    {
        while (isActive)
        {
            Vector3 newPos = transform.position + Vector3.down;
            if (IsValidPosition(newPos))
                transform.position = newPos;
            else
                break;
        }
        isActive = false;
        board.PlacePiece(transform);
    }

    private bool IsValidPosition(Vector3 position)
    {
        foreach (Transform child in transform)
        {
            Vector3 worldPos = position + (child.position - transform.position);
            Vector2Int gridPos = board.RoundToGrid(worldPos);
            if (!IsValidCell(gridPos)) return false;
        }
        return true;
    }

    private bool IsValidCell(Vector2Int pos)
    {
        return board.IsValidPosition(pos);
    }
}
