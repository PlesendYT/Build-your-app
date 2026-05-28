using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Building")]
    public string buildingName = "Building";
    public int level = 1;
    public int maxLevel = 3;
    public Sprite[] levelSprites;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

    private void OnMouseDown()
    {
        Upgrade();
    }
}
