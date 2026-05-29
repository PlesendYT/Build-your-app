/// <summary>
/// A single cell in the word search grid, displaying a letter and tracking found status.
/// </summary>
using UnityEngine;
using TMPro;

public class LetterCell : MonoBehaviour
{
    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public char letter;

    public TMP_Text letterText;
    private SpriteRenderer spriteRenderer;
    private WordGrid wordGrid;
    private bool isFound;

    /// <summary>
    /// Caches the sprite renderer and finds the letter text component.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (letterText == null) letterText = GetComponentInChildren<TMP_Text>();
    }

    /// <summary>
    /// Initializes the cell with grid position, letter, and a reference to the word grid.
    /// </summary>
    public void Init(int gridX, int gridY, char ch, WordGrid grid)
    {
        x = gridX;
        y = gridY;
        letter = ch;
        wordGrid = grid;
        if (letterText != null)
            letterText.text = ch.ToString();
        gameObject.name = "Cell_" + x + "_" + y + "_" + ch;
    }

    /// <summary>
    /// Marks the cell as found and changes its color to green.
    /// </summary>
    public void SetFound()
    {
        isFound = true;
        if (spriteRenderer != null)
            spriteRenderer.color = Color.green;
    }

    /// <summary>
    /// Logs the cell's letter on click for debugging.
    /// </summary>
    private void OnMouseDown()
    {
        if (wordGrid != null)
        {
            Debug.Log("Cell: " + letter);
        }
    }
}
