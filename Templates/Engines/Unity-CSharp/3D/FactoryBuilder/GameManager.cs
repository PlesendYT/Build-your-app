/// <summary>
/// Factory builder manager: resource tracking, building placement, automation, and production chains.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Resources")]
    public int ironOre, copperOre, coal, ironIngot, copperIngot, steelPlate, circuits, fuel;

    [Header("Buildings")]
    public int totalBuildings, activeMiners, activeSmelters, activeAssemblers;

    [Header("Power")]
    public float totalPowerGenerated, totalPowerConsumed;

    [Header("UI")]
    public TMPro.TextMeshProUGUI resourceText, powerText, buildingText;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        if (resourceText != null)
            resourceText.text = "Iron:" + ironOre + " Cu:" + copperOre + " Coal:" + coal + "\nIngot:" + ironIngot + " Plate:" + steelPlate + " Circ:" + circuits;
        if (powerText != null) powerText.text = "Power: " + Mathf.FloorToInt(totalPowerGenerated) + "/" + Mathf.FloorToInt(totalPowerConsumed) + " MW";
        if (buildingText != null) buildingText.text = "Buildings: " + totalBuildings + " | M:" + activeMiners + " S:" + activeSmelters + " A:" + activeAssemblers;
    }

    public void AddResource(string name, int amt)
    {
        switch (name) { case "IronOre": ironOre += amt; break; case "CopperOre": copperOre += amt; break; case "Coal": coal += amt; break; case "IronIngot": ironIngot += amt; break; case "CopperIngot": copperIngot += amt; break; case "SteelPlate": steelPlate += amt; break; case "Circuits": circuits += amt; break; case "Fuel": fuel += amt; break; }
    }

    public bool ConsumeResource(string name, int amt)
    {
        if (GetAmount(name) < amt) return false;
        switch (name) { case "IronOre": ironOre -= amt; break; case "CopperOre": copperOre -= amt; break; case "Coal": coal -= amt; break; case "IronIngot": ironIngot -= amt; break; case "CopperIngot": copperIngot -= amt; break; case "SteelPlate": steelPlate -= amt; break; case "Circuits": circuits -= amt; break; case "Fuel": fuel -= amt; break; }
        return true;
    }

    public int GetAmount(string name) => name switch { "IronOre" => ironOre, "CopperOre" => copperOre, "Coal" => coal, "IronIngot" => ironIngot, "CopperIngot" => copperIngot, "SteelPlate" => steelPlate, "Circuits" => circuits, "Fuel" => fuel, _ => 0 };

    public void RegisterBuilding(Building b)
    {
        totalBuildings++;
        if (b.buildingType == Building.BuildingType.Miner) activeMiners++;
        else if (b.buildingType == Building.BuildingType.Smelter) activeSmelters++;
        else if (b.buildingType == Building.BuildingType.Assembler) activeAssemblers++;
    }

    public void RestartGame() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}
