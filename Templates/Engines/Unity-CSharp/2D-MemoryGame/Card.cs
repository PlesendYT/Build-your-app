/// <summary>
/// Represents a memory card that can be revealed, hidden, and marked as matched.
/// </summary>
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

    /// <summary>
    /// Caches the sprite renderer component.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Initializes the card with its sprite index, face image, and board reference.
    /// </summary>
    public void Init(int index, Sprite face, MemoryBoard boardRef)
    {
        SpriteIndex = index;
        faceSprite = face;
        board = boardRef;
        backSprite = spriteRenderer.sprite;
        IsRevealed = false;
        IsMatched = false;
    }

    /// <summary>
    /// Reveals the card face.
    /// </summary>
    public void Reveal()
    {
        if (IsRevealed || IsMatched) return;
        IsRevealed = true;
        spriteRenderer.sprite = faceSprite;
    }

    /// <summary>
    /// Hides the card back to its back sprite.
    /// </summary>
    public void Hide()
    {
        IsRevealed = false;
        spriteRenderer.sprite = backSprite;
    }

    /// <summary>
    /// Marks the card as matched and changes its color to green.
    /// </summary>
    public void SetMatched()
    {
        IsMatched = true;
        spriteRenderer.color = Color.green;
    }

    /// <summary>
    /// Notifies the MemoryBoard when clicked.
    /// </summary>
    private void OnMouseDown()
    {
        if (board != null)
            board.CardClicked(this);
    }
}
