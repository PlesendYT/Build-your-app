using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Time")]
    public float timeOfDay;
    public float timeScale = 60f;
    public Light sunLight;
    public AnimationCurve sunIntensityCurve;

    [Header("Weather")]
    public bool isRaining;
    public ParticleSystem rainParticle;
    public AudioSource rainAudio;

    [Header("Map")]
    public Vector2 worldSize = new Vector2(1000f, 1000f);
    public Transform player;

    private void Update()
    {
        UpdateTimeOfDay();
        UpdateWeather();
    }

    private void UpdateTimeOfDay()
    {
        timeOfDay += Time.deltaTime * timeScale / 86400f;
        if (timeOfDay >= 1f) timeOfDay -= 1f;

        if (sunLight != null)
        {
            sunLight.transform.rotation = Quaternion.Euler(timeOfDay * 360f - 90f, 0, 0);
            sunLight.intensity = sunIntensityCurve.Evaluate(timeOfDay);
        }
    }

    private void UpdateWeather()
    {
        if (rainParticle != null)
        {
            if (isRaining && !rainParticle.isPlaying) rainParticle.Play();
            else if (!isRaining && rainParticle.isPlaying) rainParticle.Stop();
        }
        if (rainAudio != null)
        {
            rainAudio.volume = isRaining ? 1f : 0f;
        }
    }

    public bool IsInBounds(Vector3 position)
    {
        return Mathf.Abs(position.x) <= worldSize.x / 2f &&
               Mathf.Abs(position.z) <= worldSize.y / 2f;
    }

    public float GetTimeOfDay() => timeOfDay;
}

public class QuestManager : MonoBehaviour
{
    public void AddQuest(string questName) { }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject gameOverPanel;
    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }
    public void GameOver() { if (gameOverPanel != null) gameOverPanel.SetActive(true); Time.timeScale = 0f; }
    public void RestartGame() { Time.timeScale = 1f; UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex); }
}
