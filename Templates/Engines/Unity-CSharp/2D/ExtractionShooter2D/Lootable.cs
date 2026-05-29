/// <summary>
/// Represents a lootable object in the world that can be picked up and added to the player's inventory.
/// </summary>
using UnityEngine;

public class Lootable : MonoBehaviour
{
    [Header("Loot Data")]
    public string itemName = "Scrap";
    public int value = 10;
    public int quantity = 1;
    public Sprite lootIcon;

    [Header("Pickup")]
    public float pickupRadius = 1.5f;
    private bool isCollected;

    /// <summary>
    /// Initializes the pickup collider radius.
    /// </summary>
    private void Start()
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (col != null)
            col.radius = pickupRadius;
    }

    /// <summary>
    /// Handles visual bob animation while waiting to be picked up.
    /// </summary>
    private void Update()
    {
        if (isCollected) return;

        // Simple floating animation
        Vector3 pos = transform.position;
        pos.y += Mathf.Sin(Time.time * 2f) * 0.002f;
        transform.position = pos;
    }

    /// <summary>
    /// Prevents double-collection and marks as collected.
    /// </summary>
    public void Collect()
    {
        if (isCollected) return;
        isCollected = true;
    }

    /// <summary>
    /// Returns a formatted label showing the item and its value.
    /// </summary>
    public string GetLabel()
    {
        return itemName + " ($" + value + ")";
    }
}
