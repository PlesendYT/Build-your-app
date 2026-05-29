/// <summary>
/// Controls an active Tetris tetromino with gravity, movement, rotation, and hard/soft drop.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class TetrominoController : MonoBehaviour
{
    [Header("Movement")]
    public float fallInterval = 1f;
    private float fallTimer;
    private TetrisBoard board;
    private bool isActive = true;

    /// <summary>
    /// Finds the Tetris board reference.
    /// </summary>
    private void Awake()
    {
        board = FindObjectOfType<TetrisBoard>();
    }

    /// <summary>
    /// Initializes the fall timer.
    /// </summary>
    private void Start()
    {
        fallTimer = fallInterval;
    }

    /// <summary>
    /// Moves the piece downward automatically on a timer.
    /// </summary>
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

    /// <summary>
    /// Moves the piece left on input.
    /// </summary>
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed) Move(Vector2Int.left);
    }

    /// <summary>
    /// Moves the piece right on input.
    /// </summary>
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed) Move(Vector2Int.right);
    }

    /// <summary>
    /// Rotates the piece on input.
    /// </summary>
    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.performed) Rotate();
    }

    /// <summary>
    /// Soft drops the piece one row.
    /// </summary>
    public void OnSoftDrop(InputAction.CallbackContext context)
    {
        if (context.performed) Move(Vector2Int.down);
    }

    /// <summary>
    /// Hard drops the piece to the lowest valid position instantly.
    /// </summary>
    public void OnHardDrop(InputAction.CallbackContext context)
    {
        if (context.performed) HardDrop();
    }

    /// <summary>
    /// Moves the piece in the given direction if valid; locks it if moving down fails.
    /// </summary>
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

    /// <summary>
    /// Rotates the piece 90 degrees counter-clockwise, reversing if the rotation is invalid.
    /// </summary>
    private void Rotate()
    {
        if (!isActive) return;
        transform.Rotate(0f, 0f, -90f);
        if (!IsValidPosition(transform.position))
            transform.Rotate(0f, 0f, 90f);
    }

    /// <summary>
    /// Instantly drops the piece to the bottom and locks it.
    /// </summary>
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

    /// <summary>
    /// Checks if a position is valid by testing all child block positions against the board.
    /// </summary>
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

    /// <summary>
    /// Delegates cell validity check to the board.
    /// </summary>
    private bool IsValidCell(Vector2Int pos)
    {
        return board.IsValidPosition(pos);
    }
}
