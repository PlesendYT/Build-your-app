using UnityEngine;
using TMPro;

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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

    private void OnMouseDown()
    {
        Build();
    }
}
