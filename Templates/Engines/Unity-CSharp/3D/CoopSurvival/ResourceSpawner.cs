/// <summary>
/// Resource node spawning system with respawn timers for harvesting wood, stone, and ore.
/// </summary>
using UnityEngine;

public enum ResourceType
{
    Wood,
    Stone,
    IronOre,
    BerryBush,
    WaterSource
}

public class ResourceSpawner : MonoBehaviour
{
    [Header("Resource Settings")]
    public ResourceType resourceType = ResourceType.Wood;
    public int maxUses = 5;
    public int currentUses;
    public float respawnTime = 30f;

    [Header("Harvest Output")]
    public string itemID;
    public int minYield = 1;
    public int maxYield = 3;

    [Header("Visual")]
    public GameObject fullModel;
    public GameObject depletedModel;
    public ParticleSystem harvestEffect;

    private bool isDepleted;
    private float respawnTimer;

    private void Start()
    {
        currentUses = maxUses;
        UpdateVisual();
    }

    private void Update()
    {
        if (isDepleted)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                Respawn();
            }
        }
    }

    public void Harvest(PlayerController player)
    {
        if (isDepleted || currentUses <= 0) return;

        currentUses--;

        // Calculate yield
        int yield = Random.Range(minYield, maxYield + 1);

        // Play effects
        if (harvestEffect != null) harvestEffect.Play();

        // Grant resources to player
        player.Eat(5f); // Small hunger restoration from berries
        player.Drink(10f); // Water source

        if (currentUses <= 0)
        {
            Deplete();
        }
    }

    private void Deplete()
    {
        isDepleted = true;
        respawnTimer = respawnTime;
        UpdateVisual();
    }

    private void Respawn()
    {
        isDepleted = false;
        currentUses = maxUses;
        respawnTimer = 0f;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (fullModel != null) fullModel.SetActive(!isDepleted);
        if (depletedModel != null) depletedModel.SetActive(isDepleted);
    }

    public float GetRespawnPercent()
    {
        if (!isDepleted) return 1f;
        return 1f - (respawnTimer / respawnTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = resourceType switch
        {
            ResourceType.Wood => new Color(0.6f, 0.3f, 0f),
            ResourceType.Stone => Color.gray,
            ResourceType.IronOre => Color.cyan,
            ResourceType.BerryBush => Color.red,
            ResourceType.WaterSource => Color.blue,
            _ => Color.white
        };
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
