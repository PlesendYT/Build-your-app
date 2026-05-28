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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (letterText == null) letterText = GetComponentInChildren<TMP_Text>();
    }

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

    public void SetFound()
    {
        isFound = true;
        if (spriteRenderer != null)
            spriteRenderer.color = Color.green;
    }

    private void OnMouseDown()
    {
        if (wordGrid != null)
        {
            // Single cell selection for simple word entry
            Debug.Log("Cell: " + letter);
        }
    }
}
