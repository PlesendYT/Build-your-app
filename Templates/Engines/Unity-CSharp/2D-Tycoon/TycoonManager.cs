/// <summary>
/// Manages the tycoon economy: money, income, customer satisfaction, pricing, and UI.
/// </summary>
using UnityEngine;

public class TycoonManager : MonoBehaviour
{
    [Header("Economy")]
    public float money = 100f;
    public float incomePerSecond = 5f;
    public float customerSatisfaction = 50f;
    public TMPro.TextMeshProUGUI moneyText;
    public TMPro.TextMeshProUGUI incomeText;

    [Header("Pricing")]
    public float basePrice = 10f;
    public float priceElasticity = 0.5f;

    /// <summary>
    /// Adds passive income each frame and updates the UI.
    /// </summary>
    private void Update()
    {
        float tick = incomePerSecond * Time.deltaTime;
        money += tick;
        UpdateUI();
    }

    /// <summary>
    /// Attempts to spend money, returns false if insufficient funds.
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
    /// Multiplies the income per second by the given factor.
    /// </summary>
    public void UpgradeIncome(float multiplier)
    {
        incomePerSecond *= multiplier;
    }

    /// <summary>
    /// Adjusts the base price and recalculates income based on satisfaction elasticity.
    /// </summary>
    public void AdjustPrice(float delta)
    {
        basePrice = Mathf.Max(1f, basePrice + delta);
        incomePerSecond = basePrice * (1f + customerSatisfaction / 100f * priceElasticity);
    }

    /// <summary>
    /// Modifies satisfaction, clamped between 0 and 100.
    /// </summary>
    public void ModifySatisfaction(float delta)
    {
        customerSatisfaction = Mathf.Clamp(customerSatisfaction + delta, 0f, 100f);
    }

    /// <summary>
    /// Updates the money and income text displays.
    /// </summary>
    private void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = "Money: $" + money.ToString("F1");
        if (incomeText != null)
            incomeText.text = "Income: $" + incomePerSecond.ToString("F1") + "/s";
    }
}
