/// <summary>
/// Core simulation: ResourceManager tracks resources globally; Entity base class for living things.
/// </summary>
using UnityEngine;

public enum ResourceType
{
    Food,
    Wood,
    Stone,
    Gold
}

[System.Serializable]
public class ResourceAmount
{
    public ResourceType resourceType;
    public int amount;
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    private void Awake()
    {
        Instance = this;
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
            resources[type] = 0;
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (resources.ContainsKey(type))
            resources[type] += amount;
    }

    public bool SpendResource(ResourceType type, int amount)
    {
        if (resources.ContainsKey(type) && resources[type] >= amount)
        {
            resources[type] -= amount;
            return true;
        }
        return false;
    }

    public int GetResource(ResourceType type)
    {
        return resources.ContainsKey(type) ? resources[type] : 0;
    }
}

public abstract class Entity : MonoBehaviour
{
    public string entityName;
    public int health;
    public int maxHealth;
    public int level;

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    public abstract void Die();
}
    }

    protected virtual void MoveToDestination()
    {
        if (destination == Vector3.zero) return;

        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.5f)
        {
            state = EntityState.Idle;
            destination = Vector3.zero;
        }
    }

    public void SetDestination(Vector3 dest)
    {
        destination = dest;
        state = EntityState.Moving;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public float food;
    public float wood;
    public float stone;
    public float gold;

    private void Awake() { Instance = this; }

    public bool SpendResource(ResourceType type, float amount)
    {
        float current = GetResource(type);
        if (current >= amount)
        {
            SetResource(type, current - amount);
            return true;
        }
        return false;
    }

    public void AddResource(ResourceType type, float amount)
    {
        SetResource(type, GetResource(type) + amount);
    }

    private float GetResource(ResourceType type) => type switch
    {
        ResourceType.Food => food,
        ResourceType.Wood => wood,
        ResourceType.Stone => stone,
        ResourceType.Gold => gold,
        _ => 0
    };

    private void SetResource(ResourceType type, float value)
    {
        switch (type)
        {
            case ResourceType.Food: food = value; break;
            case ResourceType.Wood: wood = value; break;
            case ResourceType.Stone: stone = value; break;
            case ResourceType.Gold: gold = value; break;
        }
    }

    public enum ResourceType { Food, Wood, Stone, Gold }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake() { Instance = this; }
}
