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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

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

    private void OnMouseDown()
    {
        Interact();
    }
}
