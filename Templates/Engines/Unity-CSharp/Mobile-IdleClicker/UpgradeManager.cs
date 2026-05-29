using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages purchasable upgrades that enhance tap power, generator output, or apply multipliers.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    /// <summary>
    /// Represents a single upgrade with cost, type, effect, and associated UI.
    /// </summary>
    [System.Serializable]
    public class Upgrade
    {
        public string name;
        public double cost;
        public double costScaling = 1.5f;
        public UpgradeType type;
        public int targetGeneratorIndex;
        public double effectValue;
        public Button button;
        public TMPro.TextMeshProUGUI costText;
    }

    /// <summary>
    /// Enum identifying which aspect of the game this upgrade modifies.
    /// </summary>
    public enum UpgradeType { TapPower, GeneratorBoost, Multiplier }

    public Upgrade[] upgrades;

    private void Start()
    {
        // Wire up each upgrade button to the purchase handler
        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade.button != null)
                upgrade.button.onClick.AddListener(() => Purchase(upgrade));
        }
    }

    private void Update()
    {
        // Update button interactability based on affordability
        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade.button != null)
                upgrade.button.interactable = GameManager.Instance.CanAfford(upgrade.cost);
        }
    }

    /// <summary>
    /// Processes the purchase of an upgrade, applying its effect and scaling the cost.
    /// </summary>
    public void Purchase(Upgrade upgrade)
    {
        if (!GameManager.Instance.SpendResource(upgrade.cost)) return;

        TapManager tap = FindAnyObjectByType<TapManager>();
        ResourceGenerator gen = FindAnyObjectByType<ResourceGenerator>();

        // Apply the upgrade effect based on its type
        switch (upgrade.type)
        {
            case UpgradeType.TapPower:
                tap.UpgradeTapPower((float)upgrade.effectValue);
                break;
            case UpgradeType.GeneratorBoost:
                if (gen != null && upgrade.targetGeneratorIndex < gen.generators.Length)
                    gen.generators[upgrade.targetGeneratorIndex].currentProduction *= upgrade.effectValue;
                break;
            case UpgradeType.Multiplier:
                tap.MultiplyTapPower((float)upgrade.effectValue);
                break;
        }

        // Scale the cost for next purchase
        upgrade.cost *= upgrade.costScaling;
        if (upgrade.costText != null)
            upgrade.costText.text = FormatNumber(upgrade.cost);
    }

    /// <summary>
    /// Formats a large number with K, M, B suffixes.
    /// </summary>
    private string FormatNumber(double value)
    {
        if (value >= 1_000_000_000) return (value / 1_000_000_000).ToString("F1") + "B";
        if (value >= 1_000_000) return (value / 1_000_000).ToString("F1") + "M";
        if (value >= 1_000) return (value / 1_000).ToString("F1") + "K";
        return value.ToString("F0");
    }
}
