using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
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

    public enum UpgradeType { TapPower, GeneratorBoost, Multiplier }

    public Upgrade[] upgrades;

    private void Start()
    {
        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade.button != null)
                upgrade.button.onClick.AddListener(() => Purchase(upgrade));
        }
    }

    private void Update()
    {
        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade.button != null)
                upgrade.button.interactable = GameManager.Instance.CanAfford(upgrade.cost);
        }
    }

    public void Purchase(Upgrade upgrade)
    {
        if (!GameManager.Instance.SpendResource(upgrade.cost)) return;

        TapManager tap = FindAnyObjectByType<TapManager>();
        ResourceGenerator gen = FindAnyObjectByType<ResourceGenerator>();

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

        upgrade.cost *= upgrade.costScaling;
        if (upgrade.costText != null)
            upgrade.costText.text = FormatNumber(upgrade.cost);
    }

    private string FormatNumber(double value)
    {
        if (value >= 1_000_000_000) return (value / 1_000_000_000).ToString("F1") + "B";
        if (value >= 1_000_000) return (value / 1_000_000).ToString("F1") + "M";
        if (value >= 1_000) return (value / 1_000).ToString("F1") + "K";
        return value.ToString("F0");
    }
}
