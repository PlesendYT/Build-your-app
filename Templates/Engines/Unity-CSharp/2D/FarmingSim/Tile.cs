/// <summary>
/// Represents a single farm tile that can be tilled, planted, watered, and harvested.
/// </summary>
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("State")]
    public enum TileState { Empty, Tilled, Planted, Growing, Ready }
    public TileState state = TileState.Empty;

    [Header("References")]
    public SpriteRenderer tileRenderer;
    public Color tilledColor = new Color(0.5f, 0.3f, 0.15f);

    [Header("Crop")]
    public Crop plantedCrop;
    private float growthProgress;

    /// <summary>
    /// Tills the empty tile, preparing it for planting.
    /// </summary>
    public void Till()
    {
        if (state != TileState.Empty) return;
        state = TileState.Tilled;
        tileRenderer.color = tilledColor;
    }

    /// <summary>
    /// Plants a crop in the tilled tile if one is provided.
    /// </summary>
    public void Plant(Crop crop)
    {
        if (state != TileState.Tilled || crop == null) return;
        plantedCrop = crop;
        state = TileState.Planted;
        growthProgress = 0f;
        tileRenderer.color = Color.green;
    }

    /// <summary>
    /// Waters the tile, advancing growth for planted or growing crops.
    /// </summary>
    public void Water()
    {
        if (state != TileState.Planted && state != TileState.Growing) return;

        // Advance growth by a fixed amount per water
        growthProgress += 0.25f;
        if (growthProgress >= 1f)
        {
            state = TileState.Ready;
            tileRenderer.color = Color.yellow;
            Debug.Log("Crop ready to harvest!");
        }
        else
        {
            state = TileState.Growing;
            tileRenderer.color = new Color(0.3f, 0.8f, 0.3f);
        }
    }

    /// <summary>
    /// Harvests the ready crop, returning its sell value or null if not ready.
    /// </summary>
    public float Harvest()
    {
        if (state != TileState.Ready || plantedCrop == null) return 0f;

        float sellValue = plantedCrop.sellPrice;
        plantedCrop = null;
        state = TileState.Empty;
        tileRenderer.color = Color.white;

        Debug.Log("Harvested! Earned $" + sellValue);
        return sellValue;
    }

    /// <summary>
    /// Returns a description of the tile's current state.
    /// </summary>
    public string GetStatus()
    {
        switch (state)
        {
            case TileState.Empty: return "Empty";
            case TileState.Tilled: return "Tilled";
            case TileState.Planted: return "Planted: " + (plantedCrop != null ? plantedCrop.cropName : "None");
            case TileState.Growing: return "Growing: " + Mathf.FloorToInt(growthProgress * 100) + "%";
            case TileState.Ready: return "Ready to harvest!";
            default: return "Unknown";
        }
    }
}
