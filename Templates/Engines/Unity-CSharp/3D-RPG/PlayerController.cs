using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float gravity = 20f;
    private bool isGrounded;

    [Header("Stats")]
    public int level = 1;
    public int experience;
    public int experienceToNext = 100;
    public int strength = 5;
    public int dexterity = 5;
    public int intelligence = 5;
    public int maxHealth = 100;
    public int currentHealth;
    public int maxMana = 50;
    public int currentMana;

    [Header("UI")]
    public TMPro.TextMeshProUGUI levelText;
    public TMPro.TextMeshProUGUI healthText;
    public UnityEngine.UI.Slider healthSlider;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    private void Update()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        float speed = moveInput.sqrMagnitude > 0.1f ? runSpeed : walkSpeed;
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * speed;
        characterController.Move(move * Time.deltaTime);

        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        UpdateUI();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3f))
            {
                if (hit.collider.TryGetComponent<NPCController>(out var npc))
                    npc.Interact();
            }
        }
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        if (experience >= experienceToNext)
        {
            LevelUp();
        }
        UpdateUI();
    }

    private void LevelUp()
    {
        level++;
        experience -= experienceToNext;
        experienceToNext = Mathf.RoundToInt(experienceToNext * 1.5f);
        maxHealth += 20;
        maxMana += 10;
        currentHealth = maxHealth;
        currentMana = maxMana;
        strength++;
        dexterity++;
        intelligence++;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
        UpdateUI();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateUI();
    }

    public bool SpendMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    private void Die()
    {
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    private void UpdateUI()
    {
        if (levelText != null) levelText.text = "Lv. " + level;
        if (healthText != null) healthText.text = currentHealth + "/" + maxHealth;
        if (healthSlider != null) healthSlider.value = (float)currentHealth / maxHealth;
    }
}

public class NPCController : MonoBehaviour
{
    public string npcName;
    public string[] dialogueLines;

    public void Interact()
    {
        // Dialogue logic
    }
}
