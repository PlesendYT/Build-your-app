using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Match")]
    public int playerCount;
    public int aliveCount;
    public float matchTime;

    [Header("Loot")]
    public GameObject[] weaponPrefabs;
    public GameObject[] itemPrefabs;
    public int lootSpawnCount = 50;
    public Vector3 spawnArea = new Vector3(200f, 0, 200f);

    [Header("UI")]
    public TMPro.TextMeshProUGUI aliveText;
    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI killFeedText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        aliveCount = playerCount;
        SpawnLoot();
    }

    private void Update()
    {
        matchTime += Time.deltaTime;
        UpdateUI();
    }

    private void SpawnLoot()
    {
        for (int i = 0; i < lootSpawnCount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                0.5f,
                Random.Range(-spawnArea.z, spawnArea.z)
            );

            GameObject prefab = Random.value < 0.7f
                ? weaponPrefabs[Random.Range(0, weaponPrefabs.Length)]
                : itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            Instantiate(prefab, pos, Quaternion.identity);
        }
    }

    public void PlayerEliminated(PlayerController player)
    {
        aliveCount--;
        if (aliveCount <= 1)
        {
            MatchOver();
        }
        UpdateUI();
    }

    private void MatchOver()
    {
        if (aliveCount <= 1)
        {
            if (victoryPanel != null) victoryPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    private void UpdateUI()
    {
        if (aliveText != null) aliveText.text = "Alive: " + aliveCount;
        if (timerText != null) timerText.text = Mathf.FloorToInt(matchTime / 60) + ":" + (matchTime % 60).ToString("00");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
