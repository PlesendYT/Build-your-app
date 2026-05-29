/// <summary>
/// Manages city economy: money, population, happiness, zone registration, and income/expenses simulation.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class CityManager : MonoBehaviour
{
    [Header("City Stats")]
    public float money = 500f;
    public int population;
    public int maxPopulation;
    public float happiness = 50f;

    [Header("Income/Expenses")]
    public float taxRate = 0.1f;
    public float upkeepPerBuilding = 1f;

    [Header("UI")]
    public TMPro.TextMeshProUGUI moneyText;
    public TMPro.TextMeshProUGUI populationText;
    public TMPro.TextMeshProUGUI happinessText;

    private List<ZonePlot> residentialZones = new List<ZonePlot>();
    private List<ZonePlot> commercialZones = new List<ZonePlot>();
    private List<ZonePlot> industrialZones = new List<ZonePlot>();

    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static CityManager Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Calculates income and expenses each frame and updates the UI.
    /// </summary>
    private void Update()
    {
        float income = population * taxRate * 0.1f;
        float expenses = (residentialZones.Count + commercialZones.Count + industrialZones.Count) * upkeepPerBuilding * 0.1f;
        money += (income - expenses) * Time.deltaTime;

        maxPopulation = residentialZones.Count * 10;

        happiness = Mathf.Clamp(happiness + Time.deltaTime * 0.1f, 0f, 100f);

        UpdateUI();
    }

    /// <summary>
    /// Spends money if enough is available; returns true on success.
    /// </summary>
    public bool SpendMoney(float amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Registers a zone plot into the appropriate list based on its type.
    /// </summary>
    public void RegisterZone(ZonePlot zone)
    {
        switch (zone.zoneType)
        {
            case ZonePlot.ZoneType.Residential: residentialZones.Add(zone); break;
            case ZonePlot.ZoneType.Commercial: commercialZones.Add(zone); break;
            case ZonePlot.ZoneType.Industrial: industrialZones.Add(zone); break;
        }
    }

    /// <summary>
    /// Adds population up to the maximum allowed by residential zones.
    /// </summary>
    public void AddPopulation(int amount)
    {
        population = Mathf.Min(population + amount, maxPopulation);
    }

    /// <summary>
    /// Refreshes the money, population, and happiness UI displays.
    /// </summary>
    private void UpdateUI()
    {
        if (moneyText != null) moneyText.text = "Money: $" + money.ToString("F1");
        if (populationText != null) populationText.text = "Pop: " + population + "/" + maxPopulation;
        if (happinessText != null) happinessText.text = "Happiness: " + Mathf.RoundToInt(happiness) + "%";
    }
}
