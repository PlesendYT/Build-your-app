/// <summary>
/// Manages tower defense game state: money, lives, wave display, game over, and victory.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("Resources")]
    public int startMoney = 200;
    public int startLives = 20;
    private int money;
    private int lives;

    [Header("UI")]
    public TMPro.TextMeshProUGUI moneyText;
    public TMPro.TextMeshProUGUI livesText;
    public TMPro.TextMeshProUGUI waveText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Initializes resources, UI, and hides panels.
    /// </summary>
    private void Start()
    {
        money = startMoney;
        lives = startLives;
        UpdateUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    /// <summary>
    /// Attempts to spend money, returns false if insufficient funds.
    /// </summary>
    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds money and updates the display.
    /// </summary>
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    /// <summary>
    /// Reduces lives and triggers game over if lives reach zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        lives -= amount;
        UpdateUI();
        if (lives <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Updates the wave counter text.
    /// </summary>
    public void UpdateWaveText(int wave, int total)
    {
        if (waveText != null)
            waveText.text = "Wave " + wave + "/" + total;
    }

    /// <summary>
    /// Shows the game over panel and pauses the game.
    /// </summary>
    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Shows the victory panel and pauses the game.
    /// </summary>
    public void Victory()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Resumes time and reloads the current scene.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Updates the money and lives display.
    /// </summary>
    private void UpdateUI()
    {
        if (moneyText != null) moneyText.text = money.ToString();
        if (livesText != null) livesText.text = lives.ToString();
    }
}

/// <summary>
/// A projectile that travels toward a target enemy and deals damage on impact.
/// </summary>
public class Projectile : MonoBehaviour
{
    private Transform target;
    private int damage;
    public float speed = 10f;

    /// <summary>
    /// Sets the target and damage values for this projectile.
    /// </summary>
    public void Initialize(Transform target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }

    /// <summary>
    /// Moves toward the target and destroys itself upon impact.
    /// </summary>
    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
