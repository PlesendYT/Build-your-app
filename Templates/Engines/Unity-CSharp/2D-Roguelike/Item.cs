using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Health, Weapon, Armor, Key }
    public ItemType itemType;
    public string itemName;
    public int value;

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

public partial class PlayerController
{
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void UpgradeWeapon(int amount)
    {
        attackDamage += amount;
    }
}
