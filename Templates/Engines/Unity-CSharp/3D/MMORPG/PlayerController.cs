/// <summary>
/// MMORPG player with WASD movement, hotbar abilities, NPC interaction, and combat.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f, runSpeed = 10f;
    private CharacterController cc;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float gravity = 20f;

    [Header("Abilities")]
    public KeyCode[] abilityKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
    public int[] manaCosts = { 10, 20, 30, 50 }, damages = { 20, 35, 50, 80 };
    public float[] cooldowns = { 1f, 3f, 5f, 10f };
    private float[] abilityTimers;

    [Header("Interaction")]
    public float interactRange = 3f;
    public LayerMask interactLayer;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        abilityTimers = new float[abilityKeys.Length];
    }

    private void Update()
    {
        HandleMovement();
        ApplyGravity();
        HandleAbilities();
        HandleInteraction();
        RegenMana();
    }

    private void HandleMovement()
    {
        float speed = moveInput.sqrMagnitude > 0.1f ? runSpeed : moveSpeed;
        cc.Move((transform.right * moveInput.x + transform.forward * moveInput.y) * speed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (!cc.isGrounded) velocity.y -= gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2f;
        cc.Move(velocity * Time.deltaTime);
    }

    private void HandleAbilities()
    {
        for (int i = 0; i < abilityKeys.Length; i++)
        {
            if (abilityTimers[i] > 0f) abilityTimers[i] -= Time.deltaTime;
            if (Keyboard.current[abilityKeys[i]].wasPressedThisFrame && abilityTimers[i] <= 0f)
            {
                if (GameManager.Instance.playerData.mana < manaCosts[i]) return;
                GameManager.Instance.UseMana(manaCosts[i]);
                abilityTimers[i] = cooldowns[i];
                Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
                if (Physics.Raycast(r, out RaycastHit hit, 20f) && hit.collider.TryGetComponent<EnemyAI>(out EnemyAI e))
                    e.TakeDamage(damages[i]);
            }
        }
    }

    private void HandleInteraction()
    {
        if (!Keyboard.current.eKey.wasPressedThisFrame) return;
        Collider[] cols = Physics.OverlapSphere(transform.position, interactRange, interactLayer);
        foreach (Collider c in cols)
            if (c.TryGetComponent<NPCDialogue>(out NPCDialogue n)) { n.Interact(); break; }
    }

    private void RegenMana()
    {
        if (GameManager.Instance.playerData.mana < GameManager.Instance.playerData.maxMana)
            GameManager.Instance.RestoreMana(Mathf.RoundToInt(5f * Time.deltaTime));
    }

    public void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    public void TakeDamage(int dmg) => GameManager.Instance.TakeDamage(dmg);
}
