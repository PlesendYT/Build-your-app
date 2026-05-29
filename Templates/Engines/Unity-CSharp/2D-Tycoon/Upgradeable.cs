/// <summary>
/// An upgradeable item that can be leveled up to improve income, satisfaction, or capacity.
/// </summary>
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

    /// <summary>
    /// Caches the sprite renderer and finds the tycoon manager.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tycoon = FindObjectOfType<TycoonManager>();
    }

    /// <summary>
    /// Updates the visual to match the starting level.
    /// </summary>
    private void Start()
    {
        UpdateVisual();
    }

    /// <summary>
    /// Returns the current cost based on level and cost multiplier.
    /// </summary>
    public float GetCurrentCost()
    {
        return baseCost * Mathf.Pow(costMultiplier, level);
    }

    /// <summary>
    /// Returns true if the item can be upgraded (not max level and has enough money).
    /// </summary>
    public bool CanUpgrade()
    {
        return level < maxLevel && tycoon != null && tycoon.SpendMoney(GetCurrentCost());
    }

    /// <summary>
    /// Performs the upgrade: deducts cost, increases level, applies effect, and updates visual.
    /// </summary>
    public void PerformUpgrade()
    {
        if (!CanUpgrade()) return;

        // Deduct cost again with validation
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

    /// <summary>
    /// Updates the sprite to match the current upgrade level.
    /// </summary>
    private void UpdateVisual()
    {
        if (levelSprites != null && level < levelSprites.Length && levelSprites[level] != null)
            spriteRenderer.sprite = levelSprites[level];
    }

    /// <summary>
    /// Upgrades on mouse click.
    /// </summary>
    private void OnMouseDown()
    {
        PerformUpgrade();
    }
}
