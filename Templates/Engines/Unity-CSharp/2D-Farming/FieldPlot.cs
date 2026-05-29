/// <summary>
/// Represents a farm field plot with tilling, planting, watering, growth stages, and harvesting.
/// </summary>
using UnityEngine;

public class FieldPlot : MonoBehaviour
{
    [Header("State")]
    public enum PlotState { Empty, Tilled, Planted, Growing, Ready }
    public PlotState state = PlotState.Empty;
    public CropData cropData;
    public SpriteRenderer plotRenderer;

    [Header("Growth")]
    public float growthTime = 20f;
    public int waterLevel;
    public int maxWater = 3;
    private float growthTimer;

    [System.Serializable]
    public class CropData
    {
        public string cropName;
        public float sellPrice;
        public Sprite[] growthSprites;
    }

    /// <summary>
    /// Tills the empty plot, changing its color to brown.
    /// </summary>
    public void Till()
    {
        if (state == PlotState.Empty)
        {
            state = PlotState.Tilled;
            plotRenderer.color = new Color(0.6f, 0.4f, 0.2f);
        }
    }

    /// <summary>
    /// Plants a crop in a tilled plot, resetting growth timer and water level.
    /// </summary>
    public void Plant(CropData crop)
    {
        if (state == PlotState.Tilled)
        {
            cropData = crop;
            state = PlotState.Planted;
            growthTimer = 0f;
            waterLevel = 0;
            UpdateVisual();
        }
    }

    /// <summary>
    /// Increases the water level for planted or growing crops, up to the maximum.
    /// </summary>
    public void Water()
    {
        if (state == PlotState.Planted || state == PlotState.Growing)
        {
            waterLevel = Mathf.Min(waterLevel + 1, maxWater);
        }
    }

    /// <summary>
    /// Advances crop growth each frame when watered, updates visual stages, and transitions to Ready when done.
    /// </summary>
    private void Update()
    {
        if (state == PlotState.Planted || state == PlotState.Growing)
        {
            if (waterLevel > 0)
            {
                growthTimer += Time.deltaTime * (waterLevel / (float)maxWater);
                waterLevel = Mathf.Max(0, waterLevel - 1);

                if (growthTimer >= growthTime)
                {
                    state = PlotState.Ready;
                    UpdateVisual();
                }
                else if (cropData != null && cropData.growthSprites != null)
                {
                    int stage = Mathf.FloorToInt((growthTimer / growthTime) * cropData.growthSprites.Length);
                    if (stage < cropData.growthSprites.Length)
                        plotRenderer.sprite = cropData.growthSprites[stage];
                }
            }
        }
    }

    /// <summary>
    /// Harvests the ready crop, earning money and resetting the plot to empty.
    /// </summary>
    public void Harvest()
    {
        if (state == PlotState.Ready)
        {
            FarmManager farm = FindObjectOfType<FarmManager>();
            if (farm != null && cropData != null)
                farm.EarnMoney(cropData.sellPrice);

            state = PlotState.Empty;
            cropData = null;
            plotRenderer.color = Color.white;
        }
    }

    /// <summary>
    /// Updates the plot sprite to the first growth stage.
    /// </summary>
    private void UpdateVisual()
    {
        if (cropData != null && cropData.growthSprites != null && cropData.growthSprites.Length > 0)
            plotRenderer.sprite = cropData.growthSprites[0];
    }
}
