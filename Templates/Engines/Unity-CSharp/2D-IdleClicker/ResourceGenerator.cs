/// <summary>
/// Manages automatic resource generators that produce resources over time, with purchase and upgrade mechanics.
/// </summary>
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Generator
    {
        public string generatorName;
        public float baseProduction;
        public float currentProduction;
        public int level;
        public int count;
        public float baseCost;
        public float costMultiplier = 1.15f;
    }

    [Header("Generators")]
    public Generator[] generators;

    /// <summary>
    /// Initializes all generators to their base production levels.
    /// </summary>
    private void Start()
    {
        foreach (Generator gen in generators)
        {
            gen.currentProduction = gen.baseProduction;
        }
    }

    /// <summary>
    /// Adds passive resource income each frame based on all active generators.
    /// </summary>
    private void Update()
    {
        float totalProduction = 0f;
        foreach (Generator gen in generators)
        {
            totalProduction += gen.count * gen.currentProduction;
        }
        GameManager.Instance.AddResource(totalProduction * Time.deltaTime);
    }

    /// <summary>
    /// Purchases an additional copy of the specified generator.
    /// </summary>
    public bool PurchaseGenerator(int index)
    {
        if (index < 0 || index >= generators.Length) return false;

        Generator gen = generators[index];
        float cost = GetCost(index);

        if (GameManager.Instance.SpendResource(cost))
        {
            gen.count++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Upgrades the specified generator's production level.
    /// </summary>
    public bool UpgradeGenerator(int index)
    {
        if (index < 0 || index >= generators.Length) return false;

        Generator gen = generators[index];
        float upgradeCost = GetUpgradeCost(index);

        if (GameManager.Instance.SpendResource(upgradeCost))
        {
            gen.level++;
            gen.currentProduction = gen.baseProduction * Mathf.Pow(1.5f, gen.level);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns the current cost to purchase a generator, scaling with count owned.
    /// </summary>
    public float GetCost(int index)
    {
        if (index < 0 || index >= generators.Length) return float.MaxValue;
        Generator gen = generators[index];
        return gen.baseCost * Mathf.Pow(gen.costMultiplier, gen.count);
    }

    /// <summary>
    /// Returns the current cost to upgrade a generator, scaling with level.
    /// </summary>
    public float GetUpgradeCost(int index)
    {
        if (index < 0 || index >= generators.Length) return float.MaxValue;
        Generator gen = generators[index];
        return gen.baseCost * 5 * Mathf.Pow(2f, gen.level);
    }

    /// <summary>
    /// Returns the total resources generated per second across all generators.
    /// </summary>
    public float GetTotalProductionPerSecond()
    {
        float total = 0f;
        foreach (Generator gen in generators)
        {
            total += gen.count * gen.currentProduction;
        }
        return total;
    }
}
