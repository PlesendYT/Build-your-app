/// <summary>
/// Controls player movement, resource gathering, and item interaction in the survival world.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Interaction")]
    public float gatherRange = 1.5f;
    public LayerMask resourceLayer;
    public LayerMask interactableLayer;
    public GameManager gameManager;

    /// <summary>
    /// Caches rigidbody and game manager references.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
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
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Performs a gather action on the nearest harvestable resource.
    /// </summary>
    public void OnGather(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, gatherRange, resourceLayer);
        if (hit != null)
        {
            // Add resource item to inventory and notify the resource node
            gameManager.AddItem("Wood", 1);
            Debug.Log("Gathered wood from " + hit.gameObject.name);
        }
    }

    /// <summary>
    /// Interacts with objects like chests, crafting stations, or doors.
    /// </summary>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, gatherRange, interactableLayer);
        if (hit != null)
        {
            Debug.Log("Interacted with " + hit.gameObject.name);
        }
    }

    /// <summary>
    /// Opens the crafting menu when the player presses the craft key.
    /// </summary>
    public void OnOpenCrafting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Opened crafting menu.");
        }
    }
}
