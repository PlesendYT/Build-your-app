/// <summary>
/// Defines a crop type with name, growth duration, sell price, and visual stages.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "NewCrop", menuName = "Farming Sim/Crop")]
public class Crop : ScriptableObject
{
    [Header("Crop Info")]
    public string cropName = "Tomato";
    public float growthDuration = 20f;
    public float sellPrice = 15f;
    public int buyPrice = 5;

    [Header("Visual")]
    public Sprite[] growthStages;
    public Sprite harvestSprite;

    [Header("Season")]
    public Season season = Season.Spring;

    public enum Season { Spring, Summer, Fall, Winter }

    /// <summary>
    /// Returns the growth stage sprite index based on the current growth progress (0.0 to 1.0).
    /// </summary>
    public Sprite GetStageSprite(float progress)
    {
        if (growthStages == null || growthStages.Length == 0)
            return null;

        int index = Mathf.FloorToInt(progress * growthStages.Length);
        return growthStages[Mathf.Clamp(index, 0, growthStages.Length - 1)];
    }

    /// <summary>
    /// Checks if this crop can be planted in the given season.
    /// </summary>
    public bool CanPlantInSeason(string currentSeason)
    {
        return season.ToString() == currentSeason;
    }
}
