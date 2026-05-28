using UnityEngine;

public class Upgradeable : MonoBehaviour
{
    [Header("Upgrade")]
    public string itemName = "Upgrade";
    public float baseCost = 50f;
    public int level = 0;
    public int maxLevel = 5;
    public float costMultiplier = 1.8f;
    public float effectPerLevel = 1.5f;
    public Sprite[] levelSprites;
    private SpriteRenderer spriteRenderer;

    public enum UpgradeType { Income, Satisfaction, Capacity }
    public UpgradeType upgradeType;

    private TycoonManager tycoon;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tycoon = FindObjectOfType<TycoonManager>();
    }

    private void Start()
    {
        UpdateVisual();
    }

    public float GetCurrentCost()
    {
        return baseCost * Mathf.Pow(costMultiplier, level);
    }

    public bool CanUpgrade()
    {
        return level < maxLevel && tycoon != null && tycoon.SpendMoney(GetCurrentCost());
    }

    public void PerformUpgrade()
    {
        if (!CanUpgrade()) return;

        money_Sufficient:
        if (!tycoon.SpendMoney(GetCurrentCost())) return;

        level++;

        switch (upgradeType)
        {
            case UpgradeType.Income:
                tycoon.UpgradeIncome(effectPerLevel);
                break;
            case UpgradeType.Satisfaction:
                tycoon.ModifySatisfaction(10f);
                break;
            case UpgradeType.Capacity:
                tycoon.customerSatisfaction = Mathf.Min(100f, tycoon.customerSatisfaction + 5f);
                break;
        }

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (levelSprites != null && level < levelSprites.Length && levelSprites[level] != null)
            spriteRenderer.sprite = levelSprites[level];
    }

    private void OnMouseDown()
    {
        PerformUpgrade();
    }
}
