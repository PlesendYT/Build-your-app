/// <summary>
/// Day/night cycle with lighting changes, enemy spawning at night, and time tracking.
/// </summary>
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    public float dayLengthInSeconds = 600f;
    [Range(0f, 24f)] public float startHour = 6f;

    [Header("Lighting")]
    public Light sunLight;
    public Light moonLight;
    public Material skyboxMaterial;
    public AnimationCurve sunIntensityCurve;
    public Gradient skyColorGradient;

    [Header("Enemy Spawning")]
    public GameObject[] nightEnemyPrefabs;
    public float nightSpawnInterval = 5f;
    public Transform[] spawnPoints;
    private float nextSpawnTime;

    private float currentTime; // 0-24 hour scale
    private float timeSpeed;

    private void Start()
    {
        currentTime = startHour;
        timeSpeed = 24f / dayLengthInSeconds;
    }

    private void Update()
    {
        // Advance time
        currentTime += timeSpeed * Time.deltaTime;
        if (currentTime >= 24f)
        {
            currentTime = 0f;
            GameManager.Instance.dayCount++;
        }

        GameManager.Instance.timeOfDay = currentTime;
        GameManager.Instance.isNight = IsNight();

        UpdateLighting();
        HandleNightSpawning();
    }

    private void UpdateLighting()
    {
        // Normalize time to 0-1 for curves
        float t = currentTime / 24f;

        // Rotate sun based on time of day
        if (sunLight != null)
        {
            float sunAngle = (t * 360f) - 90f;
            sunLight.transform.rotation = Quaternion.Euler(sunAngle, -30f, 0f);
            sunLight.intensity = sunIntensityCurve != null ? sunIntensityCurve.Evaluate(t) : Mathf.Sin(t * Mathf.PI);
            sunLight.enabled = !IsNight();
        }

        // Toggle moon
        if (moonLight != null)
        {
            moonLight.enabled = IsNight();
        }

        // Update skybox
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetColor("_Tint", skyColorGradient.Evaluate(t));
        }
    }

    private void HandleNightSpawning()
    {
        if (!IsNight()) return;

        // Spawn enemies periodically during night
        if (Time.time >= nextSpawnTime && nightEnemyPrefabs.Length > 0 && spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject prefab = nightEnemyPrefabs[Random.Range(0, nightEnemyPrefabs.Length)];
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            nextSpawnTime = Time.time + nightSpawnInterval;
        }
    }

    public bool IsNight()
    {
        return currentTime < 6f || currentTime >= 20f;
    }

    public float GetTimeOfDay() => currentTime;

    public string GetTimeString()
    {
        int hours = Mathf.FloorToInt(currentTime);
        int minutes = Mathf.FloorToInt((currentTime - hours) * 60f);
        return hours.ToString("00") + ":" + minutes.ToString("00");
    }

    // Day event callbacks
    public void OnNewDay()
    {
        GameManager.Instance.dayCount++;
    }
}
