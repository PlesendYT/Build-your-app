using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public enum UpgradeType { ClickPower, GeneratorBoost, Multiplier }

    [Header("Upgrade Settings")]
    public UpgradeType upgradeType;
    public int targetGeneratorIndex;
    public string upgradeName;
    public float baseCost;
    public float costScaling = 1.5f;
    public float effectValue;
    public Button upgradeButton;
    public TMPro.TextMeshProUGUI costText;

    private int level;

    private void Start()
    {
        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(Purchase);
        UpdateUI();
    }

    private void Update()
    {
        if (upgradeButton != null)
            upgradeButton.interactable = GameManager.Instance.CanAfford(GetCost());
    }

    public float GetCost()
    {
        return baseCost * Mathf.Pow(costScaling, level);
    }

    public void Purchase()
    {
        float cost = GetCost();
        if (!GameManager.Instance.SpendResource(cost)) return;

        level++;

        switch (upgradeType)
        {
            case UpgradeType.ClickPower:
                FindAnyObjectByType<ClickManager>().UpgradeClickPower(effectValue);
                break;
            case UpgradeType.GeneratorBoost:
                ResourceGenerator gen = FindAnyObjectByType<ResourceGenerator>();
                if (gen != null && targetGeneratorIndex < gen.generators.Length)
                {
                    gen.generators[targetGeneratorIndex].currentProduction *= effectValue;
                }
                break;
            case UpgradeType.Multiplier:
                FindAnyObjectByType<ClickManager>().MultiplyClickPower(effectValue);
                break;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (costText != null)
            costText.text = FormatNumber(GetCost());
    }

    private string FormatNumber(float value)
    {
        if (value >= 1_000_000) return (value / 1_000_000).ToString("F1") + "M";
        if (value >= 1_000) return (value / 1_000).ToString("F1") + "K";
        return value.ToString("F0");
    }
}
