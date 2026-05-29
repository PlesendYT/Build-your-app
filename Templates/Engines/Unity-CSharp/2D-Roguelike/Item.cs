/// <summary>
/// A collectible item that heals or upgrades the player on pickup.
/// </summary>
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Health, Weapon, Armor, Key }
    public ItemType itemType;
    public string itemName;
    public int value;

    /// <summary>
    /// Applies the item's effect (heal or weapon upgrade) when the player touches it.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                switch (itemType)
                {
                    case ItemType.Health:
                        player.Heal(value);
                        break;
                    case ItemType.Weapon:
                        player.UpgradeWeapon(value);
                        break;
                }
                Destroy(gameObject);
            }
        }
    }
}

/// <summary>
/// Partial class extending PlayerController with item interaction methods.
/// </summary>
public partial class PlayerController
{
    /// <summary>
    /// Heals the player up to maximum health.
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    /// <summary>
    /// Increases the player's attack damage.
    /// </summary>
    public void UpgradeWeapon(int amount)
    {
        attackDamage += amount;
    }
}
