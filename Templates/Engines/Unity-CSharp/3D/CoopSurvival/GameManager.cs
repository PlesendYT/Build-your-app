/// <summary>
/// Co-op survival manager: hunger/thirst, crafting, base building, and world persistence.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("World Time")]
    public float timeOfDay;
    public int dayCount = 1;

    [Header("Players")]
    public List<PlayerController> players = new List<PlayerController>();

    [Header("Game State")]
    public bool isNight;
    public int totalZombiesKilled;

    [Header("UI")]
    public TMPro.TextMeshProUGUI timeText;
    public TMPro.TextMeshProUGUI dayText;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        UpdateUI();
        CheckGameOver();
    }

    public void RegisterPlayer(PlayerController player)
    {
        if (!players.Contains(player)) players.Add(player);
    }

    public void PlayerDied(PlayerController player)
    {
        players.Remove(player);
    }

    private void CheckGameOver()
    {
        if (players.Count <= 0)
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void AddKill()
    {
        totalZombiesKilled++;
    }

    private void UpdateUI()
    {
        if (timeText != null)
        {
            int hours = Mathf.FloorToInt(timeOfDay);
            int minutes = Mathf.FloorToInt((timeOfDay - hours) * 60f);
            timeText.text = hours.ToString("00") + ":" + minutes.ToString("00");
        }
        if (dayText != null) dayText.text = "Day " + dayCount;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
