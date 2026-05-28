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

    public void Till()
    {
        if (state == PlotState.Empty)
        {
            state = PlotState.Tilled;
            plotRenderer.color = new Color(0.6f, 0.4f, 0.2f);
        }
    }

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

    public void Water()
    {
        if (state == PlotState.Planted || state == PlotState.Growing)
        {
            waterLevel = Mathf.Min(waterLevel + 1, maxWater);
        }
    }

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

    private void UpdateVisual()
    {
        if (cropData != null && cropData.growthSprites != null && cropData.growthSprites.Length > 0)
            plotRenderer.sprite = cropData.growthSprites[0];
    }
}
