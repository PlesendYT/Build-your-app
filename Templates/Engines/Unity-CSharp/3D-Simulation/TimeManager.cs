/// <summary>
/// Manages in-game clock with day/hour/minute display, supports pause and speed controls.
/// </summary>
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Time")]
    public float timeScale = 1f;
    public float gameTime;
    public int day;
    public int hour;
    public int minute;

    [Header("UI")]
    public TMPro.TextMeshProUGUI timeText;

    private void Awake() { Instance = this; }

    private void Update()
    {
        gameTime += Time.deltaTime * timeScale;
        UpdateTimeOfDay();
    }

    private void UpdateTimeOfDay()
    {
        float totalMinutes = gameTime % 1440f;
        day = Mathf.FloorToInt(gameTime / 1440f) + 1;
        hour = Mathf.FloorToInt(totalMinutes / 60f);
        minute = Mathf.FloorToInt(totalMinutes % 60f);

        if (timeText != null)
            timeText.text = string.Format("Day {0} - {1:00}:{2:00}", day, hour, minute);
    }

    public void SetTimeScale(float scale)
    {
        timeScale = Mathf.Max(0, scale);
    }

    public void Pause() { timeScale = 0f; }
    public void Resume() { timeScale = 1f; }
    public void SpeedUp() { timeScale = 3f; }
}
