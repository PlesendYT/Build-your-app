/// <summary>
/// Manages farming simulation state: day/night cycle, season progression, money, and overall economy.
/// </summary>
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Time")]
    public int day = 1;
    public int seasonIndex;
    public string[] seasons = { "Spring", "Summer", "Fall", "Winter" };
    public float dayLength = 45f;
    private float dayTimer;

    [Header("Economy")]
    public float money = 200f;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;

    /// <summary>
    /// Advances the day timer each frame and triggers a new day when it expires.
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
    /// Increments the day count and cycles seasons every 28 days.
    /// </summary>
    private void AdvanceDay()
    {
        day++;
        if (day > 28)
        {
            day = 1;
            seasonIndex = (seasonIndex + 1) % seasons.Length;
            Debug.Log("Season changed to " + GetCurrentSeason());
        }
        UpdateUI();
    }

    /// <summary>
    /// Returns the current season name.
    /// </summary>
    public string GetCurrentSeason()
    {
        return seasons[seasonIndex];
    }

    /// <summary>
    /// Spends money if sufficient funds are available; returns true on success.
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
    /// Adds earned money to the player's balance.
    /// </summary>
    public void EarnMoney(float amount)
    {
        money += amount;
        UpdateUI();
    }

    /// <summary>
    /// Updates the UI text elements for money and day/season info.
    /// </summary>
    private void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = "$" + money.ToString("F0");
        if (dayText != null)
            dayText.text = GetCurrentSeason() + " - Day " + day;
    }
}
