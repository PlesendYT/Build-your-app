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

    private void Update()
    {
        dayTimer += Time.deltaTime;
        if (dayTimer >= dayLength)
        {
            dayTimer = 0f;
            AdvanceDay();
        }
    }

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

    public void EarnMoney(float amount)
    {
        money += amount;
        UpdateUI();
    }

    public string GetSeason()
    {
        return seasons[currentSeason];
    }

    private void UpdateUI()
    {
        if (moneyText != null) moneyText.text = "Money: $" + money.ToString("F1");
        if (seasonText != null) seasonText.text = GetSeason() + " - Day " + day;
    }
}
