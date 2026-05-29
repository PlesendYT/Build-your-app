/// <summary>
/// Controls a door that can be locked/unlocked with a key and transitions to the next area when entered.
/// </summary>
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public bool isLocked = true;
    public string requiredKeyId;
    public bool isBossDoor;
    public GameObject closedVisual;
    public GameObject openVisual;
    public Collider2D doorCollider;

    /// <summary>
    /// Updates the visual state on start.
    /// </summary>
    private void Start()
    {
        UpdateVisual();
    }

    /// <summary>
    /// Attempts to open the door with the given key ID. Returns true if unlocked.
    /// </summary>
    public bool TryOpen(string keyId)
    {
        if (!isLocked) return true;

        if (keyId == requiredKeyId)
        {
            isLocked = false;
            UpdateVisual();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Forces the door open without requiring a key.
    /// </summary>
    public void ForceOpen()
    {
        isLocked = false;
        UpdateVisual();
    }

    /// <summary>
    /// Updates the door's visual and collider based on locked state.
    /// </summary>
    private void UpdateVisual()
    {
        if (closedVisual != null) closedVisual.SetActive(isLocked);
        if (openVisual != null) openVisual.SetActive(!isLocked);
        if (doorCollider != null) doorCollider.enabled = isLocked;
    }

    /// <summary>
    /// Loads the next area when the player enters an unlocked door.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLocked)
        {
            GameManager.Instance.LoadNextArea();
        }
    }
}
