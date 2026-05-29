/// <summary>
/// Represents a building that can be upgraded with money, changing its sprite at each level.
/// </summary>
using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Building")]
    public string buildingName = "Building";
    public int level = 1;
    public int maxLevel = 3;
    public Sprite[] levelSprites;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Caches the sprite renderer component.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Upgrades the building if below max level and enough money is available, updating the sprite.
    /// </summary>
    public void Upgrade()
    {
        if (level >= maxLevel) return;

        CityManager city = CityManager.Instance;
        float cost = level * 100f;
        if (city == null || !city.SpendMoney(cost)) return;

        level++;
        if (levelSprites != null && level - 1 < levelSprites.Length)
            spriteRenderer.sprite = levelSprites[level - 1];
    }

    /// <summary>
    /// Upgrades the building on mouse click.
    /// </summary>
    private void OnMouseDown()
    {
        Upgrade();
    }
}
