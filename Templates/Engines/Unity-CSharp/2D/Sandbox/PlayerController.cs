/// <summary>
/// Controls sandbox player movement, camera follow, and block interaction (place/destroy).
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Interaction")]
    public float reachRange = 5f;
    public Block.BlockType selectedBlockType = Block.BlockType.Dirt;

    [Header("References")]
    public WorldManager worldManager;
    public Camera playerCamera;

    /// <summary>
    /// Caches component references.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        worldManager = FindObjectOfType<WorldManager>();
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    /// <summary>
    /// Moves the camera to follow the player.
    /// </summary>
    private void LateUpdate()
    {
        if (playerCamera != null)
            playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    /// <summary>
    /// Applies movement velocity each physics frame.
    /// </summary>
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    /// <summary>
    /// Reads movement input from the Input System.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context) { moveInput = context.ReadValue<Vector2>(); }

    /// <summary>
    /// Places a block at the mouse cursor position in the world.
    /// </summary>
    public void OnPlaceBlock(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorld.z = 0;
        Vector2Int gridPos = worldManager.WorldToGrid(mouseWorld);
        if (Vector2.Distance(transform.position, new Vector2(gridPos.x, gridPos.y)) <= reachRange)
            worldManager.PlaceBlock(gridPos, selectedBlockType);
    }

    /// <summary>
    /// Destroys the block at the mouse cursor position.
    /// </summary>
    public void OnDestroyBlock(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorld.z = 0;
        Vector2Int gridPos = worldManager.WorldToGrid(mouseWorld);
        if (Vector2.Distance(transform.position, new Vector2(gridPos.x, gridPos.y)) <= reachRange)
            worldManager.DestroyBlock(gridPos);
    }

    /// <summary>
    /// Cycles the selected block type for placement.
    /// </summary>
    public void OnCycleBlock(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        selectedBlockType = (Block.BlockType)(((int)selectedBlockType + 1) % 6);
    }
}
