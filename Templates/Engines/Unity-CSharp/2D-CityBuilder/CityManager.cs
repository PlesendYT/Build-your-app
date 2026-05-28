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

    public static CityManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        float income = population * taxRate * 0.1f;
        float expenses = (residentialZones.Count + commercialZones.Count + industrialZones.Count) * upkeepPerBuilding * 0.1f;
        money += (income - expenses) * Time.deltaTime;

        maxPopulation = residentialZones.Count * 10;

        happiness = Mathf.Clamp(happiness + Time.deltaTime * 0.1f, 0f, 100f);

        UpdateUI();
    }

    public bool SpendMoney(float amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false;
    }

    public void RegisterZone(ZonePlot zone)
    {
        switch (zone.zoneType)
        {
            case ZonePlot.ZoneType.Residential: residentialZones.Add(zone); break;
            case ZonePlot.ZoneType.Commercial: commercialZones.Add(zone); break;
            case ZonePlot.ZoneType.Industrial: industrialZones.Add(zone); break;
        }
    }

    public void AddPopulation(int amount)
    {
        population = Mathf.Min(population + amount, maxPopulation);
    }

    private void UpdateUI()
    {
        if (moneyText != null) moneyText.text = "Money: $" + money.ToString("F1");
        if (populationText != null) populationText.text = "Pop: " + population + "/" + maxPopulation;
        if (happinessText != null) happinessText.text = "Happiness: " + Mathf.RoundToInt(happiness) + "%";
    }
}
