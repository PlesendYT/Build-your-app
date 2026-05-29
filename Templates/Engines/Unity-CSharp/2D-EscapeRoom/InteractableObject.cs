/// <summary>
/// Represents an interactable object in the escape room that can be picked up, combined, or examined.
/// </summary>
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction")]
    public string objectName = "Object";
    public string requiredItem;
    public bool isPickup;
    public string giveItem;
    public string interactMessage;
    public bool destroyOnUse;

    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    /// <summary>
    /// Caches sprite renderer and collider components.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Handles interaction: pickups, combining items with required item, or showing a message.
    /// </summary>
    public void Interact()
    {
        EscapeRoomManager rm = EscapeRoomManager.Instance;
        if (rm == null) return;

        if (isPickup)
        {
            rm.PickupItem(giveItem);
            if (destroyOnUse) Destroy(gameObject);
            return;
        }

        // Check if player has the required item to combine
        if (!string.IsNullOrEmpty(requiredItem))
        {
            if (rm.HasItem(requiredItem))
            {
                rm.UseItem(requiredItem);
                rm.TryCombine(requiredItem, objectName);
                if (destroyOnUse) Destroy(gameObject);
            }
            else
            {
                rm.ShowMessage("I need something else...");
            }
            return;
        }

        if (!string.IsNullOrEmpty(interactMessage))
            rm.ShowMessage(interactMessage);

        if (!string.IsNullOrEmpty(giveItem))
            rm.PickupItem(giveItem);

        if (destroyOnUse) Destroy(gameObject);
    }

    /// <summary>
    /// Interacts with the object when clicked.
    /// </summary>
    private void OnMouseDown()
    {
        Interact();
    }
}
