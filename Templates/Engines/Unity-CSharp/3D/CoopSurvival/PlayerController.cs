/// <summary>
/// Co-op survival player with hunger/thirst, inventory, building placement, and tool usage.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f, sprintSpeed = 8f;
    private CharacterController cc;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float gravity = 20f;

    [Header("Stats")]
    public float maxHealth = 100f, currentHealth, maxHunger = 100f, currentHunger, maxThirst = 100f, currentThirst;

    [Header("Building")]
    public GameObject[] buildingPrefabs;
    public int selectedBuildingIndex = -1;
    public float placementRange = 5f;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        currentHealth = maxHealth; currentHunger = maxHunger; currentThirst = maxThirst;
        GameManager.Instance.RegisterPlayer(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleSurvival();
        HandleBuildingPlacement();
    }

    private void HandleMovement()
    {
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y);
        float speed = moveInput.sqrMagnitude > 0.1f ? sprintSpeed : walkSpeed;
        cc.Move(move * speed * Time.deltaTime);
        if (!cc.isGrounded) velocity.y -= gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2f;
        cc.Move(velocity * Time.deltaTime);
    }

    private void HandleSurvival()
    {
        currentHunger = Mathf.Max(0, currentHunger - 1.5f * Time.deltaTime);
        currentThirst = Mathf.Max(0, currentThirst - 2.5f * Time.deltaTime);
        if (currentHunger <= 0 || currentThirst <= 0) currentHealth -= 3f * Time.deltaTime;
        if (currentHealth <= 0) { GameManager.Instance.PlayerDied(this); gameObject.SetActive(false); }
    }

    private void HandleBuildingPlacement()
    {
        if (selectedBuildingIndex < 0 || selectedBuildingIndex >= buildingPrefabs.Length) return;
        if (Mouse.current.leftButton.wasPressedThisFrame &&
            Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit, placementRange))
            Instantiate(buildingPrefabs[selectedBuildingIndex], hit.point, Quaternion.identity);
    }

    public void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        Collider[] cols = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider c in cols)
            if (c.TryGetComponent<ResourceSpawner>(out ResourceSpawner r)) { r.Harvest(this); break; }
    }

    public void Eat(float amt) => currentHunger = Mathf.Min(maxHunger, currentHunger + amt);
    public void Drink(float amt) => currentThirst = Mathf.Min(maxThirst, currentThirst + amt);
    public void Heal(float amt) => currentHealth = Mathf.Min(maxHealth, currentHealth + amt);
}
