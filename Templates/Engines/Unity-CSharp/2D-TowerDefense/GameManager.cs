using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

    private void Start()
    {
        money = startMoney;
        lives = startLives;
        UpdateUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

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

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        lives -= amount;
        UpdateUI();
        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void UpdateWaveText(int wave, int total)
    {
        if (waveText != null)
            waveText.text = "Wave " + wave + "/" + total;
    }

    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Victory()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateUI()
    {
        if (moneyText != null) moneyText.text = money.ToString();
        if (livesText != null) livesText.text = lives.ToString();
    }
}

public class Projectile : MonoBehaviour
{
    private Transform target;
    private int damage;
    public float speed = 10f;

    public void Initialize(Transform target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }

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
