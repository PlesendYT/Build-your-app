using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity")]
    public string entityName;
    public EntityType type;
    public EntityState state;
    public float health = 100f;
    public float maxHealth = 100f;

    [Header("Behavior")]
    public float moveSpeed = 3f;
    public Transform target;
    public Vector3 destination;

    public enum EntityType { Person, Animal, Vehicle, Building }
    public enum EntityState { Idle, Moving, Working, Sleeping }

    protected virtual void Update()
    {
        switch (state)
        {
            case EntityState.Moving:
                MoveToDestination();
                break;
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
