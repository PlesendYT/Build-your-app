using UnityEngine;

public class HiddenItem : MonoBehaviour
{
    [Header("Item Data")]
    public string itemName = "Unnamed Item";
    public Sprite foundSprite;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private bool isFound;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        gameObject.SetActive(false);
    }

    public void OnFound()
    {
        if (isFound) return;
        isFound = true;
        if (foundSprite != null)
            spriteRenderer.sprite = foundSprite;
        col.enabled = false;
        transform.localScale = Vector3.one * 1.2f;
    }

    public void FlashHint()
    {
        StartCoroutine(FlashRoutine());
    }

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
