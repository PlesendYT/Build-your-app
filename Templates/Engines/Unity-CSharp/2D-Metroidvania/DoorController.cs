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

    private void Start()
    {
        UpdateVisual();
    }

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

    public void ForceOpen()
    {
        isLocked = false;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (closedVisual != null) closedVisual.SetActive(isLocked);
        if (openVisual != null) openVisual.SetActive(!isLocked);
        if (doorCollider != null) doorCollider.enabled = isLocked;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLocked)
        {
            GameManager.Instance.LoadNextArea();
        }
    }
}
