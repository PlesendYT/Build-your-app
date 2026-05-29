/// <summary>
/// Represents a buildable zone plot that can be developed with a building on click.
/// </summary>
using UnityEngine;

public class ZonePlot : MonoBehaviour
{
    [Header("Zone")]
    public enum ZoneType { Residential, Commercial, Industrial }
    public ZoneType zoneType;
    public GameObject buildingPrefab;
    public float buildCost = 50f;

    private SpriteRenderer spriteRenderer;
    private GameObject building;
    private bool isBuilt;

    /// <summary>
    /// Caches the sprite renderer component.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Constructs a building on this plot if not already built and enough money is available.
    /// </summary>
    public bool Build()
    {
        if (isBuilt) return false;

        CityManager city = CityManager.Instance;
        if (city == null || !city.SpendMoney(buildCost)) return false;

        isBuilt = true;
        if (buildingPrefab != null)
            building = Instantiate(buildingPrefab, transform.position, Quaternion.identity, transform);

        city.RegisterZone(this);
        city.AddPopulation(zoneType == ZoneType.Residential ? 10 : 5);
        spriteRenderer.color = Color.gray;
        return true;
    }

    /// <summary>
    /// Builds on this plot when clicked.
    /// </summary>
    private void OnMouseDown()
    {
        Build();
    }
}
