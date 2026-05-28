using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public int type;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Init(int gridX, int gridY, int tileType)
    {
        x = gridX;
        y = gridY;
        type = tileType;
    }

    public void MoveTo(int newX, int newY)
    {
        x = newX;
        y = newY;
        transform.position = new Vector3(newX, newY, 0f) + (transform.parent != null ? transform.parent.position : Vector3.zero);
    }

    public void SetSelected(bool selected)
    {
        spriteRenderer.color = selected ? Color.yellow : originalColor;
    }

    private void OnMouseDown()
    {
        BoardManager board = FindObjectOfType<BoardManager>();
        if (board != null)
            board.SelectTile(this);
    }
}
