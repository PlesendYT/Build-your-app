/// <summary>
/// Tactical shooter player with leaning, aiming, cover mechanics, and team identification.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f, crouchSpeed = 2f, aimSpeed = 2.5f;
    private CharacterController cc;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float gravity = 20f;
    private bool isCrouching;

    [Header("Aiming")]
    public bool isAiming;
    public float aimFOV = 40f, normalFOV = 60f;
    public Camera playerCam;

    [Header("Leaning")]
    public float leanAngle = 30f, leanSpeed = 5f;
    private float currentLean;
    private int leanDir;

    [Header("Combat")]
    public int maxHealth = 100, currentHealth, damage = 30;
    public float fireRate = 0.15f;
    public Transform shootOrigin;
    private float nextFireTime;

    [Header("Team")]
    public Team team = Team.Attackers;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        GameManager.Instance.RegisterPlayer(this, team);
    }

    private void Update()
    {
        HandleMovement();
        HandleLeaning();
        HandleAiming();
        velocity.y -= gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    private void HandleMovement()
    {
        float speed = isCrouching ? crouchSpeed : isAiming ? aimSpeed : walkSpeed;
        cc.Move((transform.right * moveInput.x + transform.forward * moveInput.y) * speed * Time.deltaTime);
    }

    private void HandleLeaning()
    {
        currentLean = Mathf.Lerp(currentLean, leanDir * leanAngle, leanSpeed * Time.deltaTime);
        if (playerCam != null) { Vector3 p = playerCam.transform.localPosition; p.x = currentLean * 0.01f; playerCam.transform.localPosition = p; }
    }

    private void HandleAiming()
    {
        if (playerCam != null)
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, isAiming ? aimFOV : normalFOV, 10f * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    public void OnCrouch(InputAction.CallbackContext ctx) { if (ctx.performed) isCrouching = !isCrouching; }
    public void OnAim(InputAction.CallbackContext ctx) => isAiming = ctx.ReadValue<float>() > 0.5f;
    public void OnLeanLeft(InputAction.CallbackContext ctx) { if (ctx.performed) leanDir = leanDir == -1 ? 0 : -1; }
    public void OnLeanRight(InputAction.CallbackContext ctx) { if (ctx.performed) leanDir = leanDir == 1 ? 0 : 1; }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            if (Physics.Raycast(shootOrigin.position, shootOrigin.forward, out RaycastHit hit, 100f))
                if (hit.collider.TryGetComponent<PlayerController>(out PlayerController p) && p.team != team)
                    p.TakeDamage(damage);
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0) { GameManager.Instance.PlayerEliminated(this); gameObject.SetActive(false); }
    }
}
