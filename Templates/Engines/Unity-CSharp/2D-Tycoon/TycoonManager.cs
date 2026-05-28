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

    private void Update()
    {
        float tick = incomePerSecond * Time.deltaTime;
        money += tick;
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

    public void UpgradeIncome(float multiplier)
    {
        incomePerSecond *= multiplier;
    }

    public void AdjustPrice(float delta)
    {
        basePrice = Mathf.Max(1f, basePrice + delta);
        incomePerSecond = basePrice * (1f + customerSatisfaction / 100f * priceElasticity);
    }

    public void ModifySatisfaction(float delta)
    {
        customerSatisfaction = Mathf.Clamp(customerSatisfaction + delta, 0f, 100f);
    }

    private void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = "Money: $" + money.ToString("F1");
        if (incomeText != null)
            incomeText.text = "Income: $" + incomePerSecond.ToString("F1") + "/s";
    }
}
