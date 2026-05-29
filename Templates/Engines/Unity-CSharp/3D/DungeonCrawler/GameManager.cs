/// <summary>
/// Dungeon crawler manager handling room generation, combat state, and player progression.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Dungeon")]
    public int currentFloor = 1;
    public int maxFloors = 10;
    public int roomsCleared;

    [Header("Player State")]
    public int playerHealth = 100;
    public int playerMaxHealth = 100;
    public int gold;
    public int keys;

    [Header("Combat")]
    public int enemiesOnFloor;
    public int enemiesDefeated;

    [Header("UI")]
    public TMPro.TextMeshProUGUI floorText;
    public TMPro.TextMeshProUGUI healthText;
    public TMPro.TextMeshProUGUI goldText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        UpdateUI();
    }

    public void EnemyDefeated(int goldReward)
    {
        enemiesDefeated++;
        gold += goldReward;

        // Check if floor is cleared
        if (enemiesDefeated >= enemiesOnFloor)
        {
            FloorCleared();
        }
    }

    private void FloorCleared()
    {
        roomsCleared++;

        // Find exit door and activate it
        DungeonGenerator generator = FindObjectOfType<DungeonGenerator>();
        if (generator != null) generator.ActivateExit();
    }

    public void DescendFloor()
    {
        currentFloor++;
        enemiesDefeated = 0;
        enemiesOnFloor = 5 + (currentFloor * 2);

        // Regenerate dungeon
        DungeonGenerator generator = FindObjectOfType<DungeonGenerator>();
        if (generator != null) generator.GenerateDungeon();
    }

    public void PlayerTakeDamage(int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0) PlayerDied();
    }

    public void HealPlayer(int amount)
    {
        playerHealth = Mathf.Min(playerMaxHealth, playerHealth + amount);
    }

    private void PlayerDied()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Victory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void UpdateUI()
    {
        if (floorText != null) floorText.text = "Floor " + currentFloor;
        if (healthText != null) healthText.text = "HP: " + playerHealth + "/" + playerMaxHealth;
        if (goldText != null) goldText.text = "Gold: " + gold;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
