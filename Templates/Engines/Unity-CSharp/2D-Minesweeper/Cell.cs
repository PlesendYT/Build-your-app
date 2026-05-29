/// <summary>
/// Represents a single cell on the minesweeper board with mine state, adjacent count, reveal, and flagging.
/// </summary>
/// <summary>
/// A single cell on the Minesweeper board with reveal, flag, and visual state management.
/// </summary>
using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer spriteRenderer;
    public TMP_Text labelText;

    [Header("State")]
    public int AdjacentMines { get; set; }
    public bool HasMine { get; set; }
    public bool IsRevealed { get; private set; }
    public bool IsFlagged { get; private set; }

    public int GridX { get; private set; }
    public int GridY { get; private set; }
    private MinesweeperBoard board;

    public Sprite[] numberSprites;
    public Sprite mineSprite;
    public Sprite flagSprite;
    public Sprite unrevealedSprite;

    /// <summary>
    /// Initializes the cell with grid position, board reference, and default visuals.
    /// </summary>
    public void Init(int x, int y, MinesweeperBoard b)
    {
        GridX = x;
        GridY = y;
        board = b;
        IsRevealed = false;
        IsFlagged = false;
        spriteRenderer.sprite = unrevealedSprite;
        labelText.text = "";
    }

    /// <summary>
    /// Reveals the cell, showing mine, number, or empty state.
    /// </summary>
    public void Reveal()
    {
        if (IsRevealed || IsFlagged) return;
        IsRevealed = true;

        if (HasMine)
        {
            spriteRenderer.sprite = mineSprite;
            return;
        }

        if (AdjacentMines > 0 && numberSprites != null && AdjacentMines <= numberSprites.Length)
        {
            spriteRenderer.sprite = numberSprites[AdjacentMines - 1];
        }
        else
        {
            spriteRenderer.sprite = null;
        }
        labelText.text = AdjacentMines > 0 ? AdjacentMines.ToString() : "";
    }

    /// <summary>
    /// Toggles the flagged state (right-click).
    /// </summary>
    public void ToggleFlag()
    {
        if (IsRevealed) return;
        IsFlagged = !IsFlagged;
        spriteRenderer.sprite = IsFlagged ? flagSprite : unrevealedSprite;
    }

    /// <summary>
    /// Left-click reveals the cell via the board.
    /// </summary>
    private void OnMouseDown()
    {
        if (IsRevealed || IsFlagged) return;
        board.RevealCell(GridX, GridY);
    }

    /// <summary>
    /// Right-click toggles a flag on the cell.
    /// </summary>
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            board.ToggleFlag(GridX, GridY);
        }
    }
}

    /// <summary>
    /// Initializes the cell with grid coordinates and board reference.
    /// </summary>
    public void Init(int gridX, int gridY, MinesweeperBoard b)
    {
        x = gridX;
        y = gridY;
        board = b;
    }

    /// <summary>
    /// Reveals the cell: shows mine (red M) or adjacent mine count.
    /// </summary>
    public void Reveal()
    {
        if (IsRevealed || IsFlagged) return;
        IsRevealed = true;

        if (HasMine)
        {
            spriteRenderer.color = Color.red;
            labelText.text = "M";
        }
        else
        {
            spriteRenderer.color = Color.gray;
            if (AdjacentMines > 0)
                labelText.text = AdjacentMines.ToString();
            else
                labelText.text = "";
        }
    }

    /// <summary>
    /// Toggles a flag on the cell (right-click).
    /// </summary>
    public void ToggleFlag()
    {
        if (IsRevealed) return;
        IsFlagged = !IsFlagged;
        labelText.text = IsFlagged ? "F" : "";
        spriteRenderer.color = IsFlagged ? Color.yellow : Color.white;
    }

    /// <summary>
    /// Handles left-click (reveal) and right-click (flag) input.
    /// </summary>
    private void OnMouseDown()
    {
        if (board != null)
        {
            if (Input.GetMouseButtonDown(1))
                board.ToggleFlag(x, y);
            else
                board.RevealCell(x, y);
        }
    }
}
