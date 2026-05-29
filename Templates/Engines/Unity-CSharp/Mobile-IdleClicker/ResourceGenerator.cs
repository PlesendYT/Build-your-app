using UnityEngine;

/// <summary>
/// Manages automatic resource generation, offline earnings, and generator purchases/upgrades.
/// </summary>
public class ResourceGenerator : MonoBehaviour
{
    /// <summary>
    /// Represents a single generator type with production, level, and cost properties.
    /// </summary>
    [System.Serializable]
    public class Generator
    {
        public string name;
        public double baseProduction;
        public double currentProduction;
        public int level;
        public int count;
        public double baseCost;
        public double costMultiplier = 1.15f;
    }

    [Header("Generators")]
    public Generator[] generators;

    [Header("Offline")]
    private double offlineEarnings;
    private string lastLoginKey = "LastLogin";

    private void Start()
    {
        CalculateOfflineEarnings();
        // Initialize each generator's current production to its base value
        foreach (Generator gen in generators)
            gen.currentProduction = gen.baseProduction;
    }

    private void Update()
    {
        // Calculate total production per second across all generators
        double total = 0;
        foreach (Generator gen in generators)
            total += gen.count * gen.currentProduction;

        // Add production scaled by frame time
        GameManager.Instance.AddResource(total * Time.deltaTime);
    }

    /// <summary>
    /// Calculates offline earnings based on time elapsed since last login and adds them.
    /// </summary>
    private void CalculateOfflineEarnings()
    {
        if (PlayerPrefs.HasKey(lastLoginKey))
        {
            long lastLogin = long.Parse(PlayerPrefs.GetString(lastLoginKey));
            System.DateTime lastTime = System.DateTime.FromBinary(lastLogin);
            double secondsOffline = (System.DateTime.Now - lastTime).TotalSeconds;

            if (secondsOffline > 0)
            {
                // Compute total production rate and scale by half for offline time
                double totalProduction = 0;
                foreach (Generator gen in generators)
                    totalProduction += gen.count * gen.currentProduction;

                offlineEarnings = totalProduction * secondsOffline * 0.5;
                if (offlineEarnings > 0)
                {
                    GameManager.Instance.AddResource(offlineEarnings);
                    GameManager.Instance.ShowOfflineEarnings(offlineEarnings);
                }
            }
        }

        // Save current time as last login
        PlayerPrefs.SetString(lastLoginKey, System.DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Buys another unit of the generator at the given index. Returns true if successful.
    /// </summary>
    public bool BuyGenerator(int index)
    {
        if (index < 0 || index >= generators.Length) return false;

        Generator gen = generators[index];
        double cost = gen.baseCost * System.Math.Pow(gen.costMultiplier, gen.count);

        if (GameManager.Instance.SpendResource(cost))
        {
            gen.count++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Upgrades the generator at the given index, increasing its production multiplier.
    /// </summary>
    public bool UpgradeGenerator(int index)
    {
        if (index < 0 || index >= generators.Length) return false;

        Generator gen = generators[index];
        double cost = gen.baseCost * 5 * System.Math.Pow(2, gen.level);

        if (GameManager.Instance.SpendResource(cost))
        {
            gen.level++;
            gen.currentProduction = gen.baseProduction * System.Math.Pow(1.5, gen.level);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns the cost to buy another unit of the generator at the given index.
    /// </summary>
    public double GetCost(int index)
    {
        if (index < 0 || index >= generators.Length) return double.MaxValue;
        Generator gen = generators[index];
        return gen.baseCost * System.Math.Pow(gen.costMultiplier, gen.count);
    }

    /// <summary>
    /// Returns the total production per second from all generators.
    /// </summary>
    public double GetTotalProduction()
    {
        double total = 0;
        foreach (Generator gen in generators)
            total += gen.count * gen.currentProduction;
        return total;
    }
}
