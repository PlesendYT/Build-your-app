/// <summary>
/// Represents a hidden item that can be found by clicking, with visual feedback and hint flashing.
/// </summary>
using UnityEngine;

public class HiddenItem : MonoBehaviour
{
    [Header("Item Data")]
    public string itemName = "Unnamed Item";
    public Sprite foundSprite;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private bool isFound;

    /// <summary>
    /// Caches components and starts inactive until selected by the scene manager.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when the item is found: updates sprite, disables collider, and scales up.
    /// </summary>
    public void OnFound()
    {
        if (isFound) return;
        isFound = true;
        if (foundSprite != null)
            spriteRenderer.sprite = foundSprite;
        col.enabled = false;
        transform.localScale = Vector3.one * 1.2f;
    }

    /// <summary>
    /// Starts a flashing visual effect to hint at the item's location.
    /// </summary>
    public void FlashHint()
    {
        StartCoroutine(FlashRoutine());
    }

    /// <summary>
    /// Flashes the item yellow three times.
    /// </summary>
    private System.Collections.IEnumerator FlashRoutine()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
    }

    /// <summary>
    /// Reports being found to the parent scene when clicked.
    /// </summary>
    private void OnMouseDown()
    {
        if (!isFound)
        {
            HiddenObjectScene scene = GetComponentInParent<HiddenObjectScene>();
            if (scene != null)
                scene.ItemFound(this);
        }
    }
}
