/// <summary>
/// Manages farm economy, day/night cycle, and season progression.
/// </summary>
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    [Header("Economy")]
    public float money = 100f;
    public TMPro.TextMeshProUGUI moneyText;
    public TMPro.TextMeshProUGUI seasonText;

    [Header("Seasons")]
    public string[] seasons = { "Spring", "Summer", "Fall", "Winter" };
    public int currentSeason;
    public int day;
    public float dayLength = 30f;
    private float dayTimer;

    /// <summary>
    /// Advances the day timer each frame and triggers day change when the timer expires.
    /// </summary>
    private void Update()
    {
        dayTimer += Time.deltaTime;
        if (dayTimer >= dayLength)
        {
            dayTimer = 0f;
            AdvanceDay();
        }
    }

    /// <summary>
    /// Increments the day and cycles seasons after 30 days.
    /// </summary>
    private void AdvanceDay()
    {
        day++;
        if (day > 30)
        {
            day = 1;
            currentSeason = (currentSeason + 1) % seasons.Length;
        }
        UpdateUI();
    }

    /// <summary>
    /// Spends money if enough is available; returns true on success.
    /// </summary>
    public bool SpendMoney(float amount)
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
    /// Adds earned money to the balance.
    /// </summary>
    public void EarnMoney(float amount)
    {
        money += amount;
        UpdateUI();
    }

    /// <summary>
    /// Returns the current season name.
    /// </summary>
    public string GetSeason()
    {
        return seasons[currentSeason];
    }

    /// <summary>
    /// Updates the money and season/day text displays.
    /// </summary>
    private void UpdateUI()
    {
        if (moneyText != null) moneyText.text = "Money: $" + money.ToString("F1");
        if (seasonText != null) seasonText.text = GetSeason() + " - Day " + day;
    }
}
