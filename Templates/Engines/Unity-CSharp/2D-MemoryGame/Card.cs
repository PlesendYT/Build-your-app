using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Card Data")]
    public int SpriteIndex { get; private set; }
    public bool IsRevealed { get; private set; }
    public bool IsMatched { get; private set; }

    private SpriteRenderer spriteRenderer;
    private Sprite faceSprite;
    private Sprite backSprite;
    private MemoryBoard board;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(int index, Sprite face, MemoryBoard boardRef)
    {
        SpriteIndex = index;
        faceSprite = face;
        board = boardRef;
        backSprite = spriteRenderer.sprite;
        IsRevealed = false;
        IsMatched = false;
    }

    public void Reveal()
    {
        if (IsRevealed || IsMatched) return;
        IsRevealed = true;
        spriteRenderer.sprite = faceSprite;
    }

    public void Hide()
    {
        IsRevealed = false;
        spriteRenderer.sprite = backSprite;
    }

    public void SetMatched()
    {
        IsMatched = true;
        spriteRenderer.color = Color.green;
    }

    private void OnMouseDown()
    {
        if (board != null)
            board.CardClicked(this);
    }
}
