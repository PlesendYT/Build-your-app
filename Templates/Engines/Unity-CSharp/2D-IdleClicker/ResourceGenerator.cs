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

    private void Update()
    {
        float totalProduction = 0f;
        foreach (Generator gen in generators)
        {
            totalProduction += gen.count * gen.currentProduction;
        }
        GameManager.Instance.AddResource(totalProduction * Time.deltaTime);
    }

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

    public float GetCost(int index)
    {
        if (index < 0 || index >= generators.Length) return float.MaxValue;
        Generator gen = generators[index];
        return gen.baseCost * Mathf.Pow(gen.costMultiplier, gen.count);
    }

    public float GetUpgradeCost(int index)
    {
        if (index < 0 || index >= generators.Length) return float.MaxValue;
        Generator gen = generators[index];
        return gen.baseCost * 5 * Mathf.Pow(2f, gen.level);
    }

    private void Start()
    {
        foreach (Generator gen in generators)
        {
            gen.currentProduction = gen.baseProduction;
        }
    }

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
