using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public bool HasMine;
    [HideInInspector] public int AdjacentMines;
    public bool IsRevealed { get; private set; }
    public bool IsFlagged { get; private set; }

    public TMP_Text labelText;
    private SpriteRenderer spriteRenderer;
    private MinesweeperBoard board;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (labelText == null) labelText = GetComponentInChildren<TMP_Text>();
    }

    public void Init(int gridX, int gridY, MinesweeperBoard b)
    {
        x = gridX;
        y = gridY;
        board = b;
    }

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

    public void ToggleFlag()
    {
        if (IsRevealed) return;
        IsFlagged = !IsFlagged;
        labelText.text = IsFlagged ? "F" : "";
        spriteRenderer.color = IsFlagged ? Color.yellow : Color.white;
    }

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
