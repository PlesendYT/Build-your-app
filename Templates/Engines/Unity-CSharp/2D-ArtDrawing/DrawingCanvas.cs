/// <summary>
/// Manages the drawing canvas, handling texture creation, pixel painting, and undo functionality.
/// </summary>
using UnityEngine;

public class DrawingCanvas : MonoBehaviour
{
    [Header("Canvas")]
    public int canvasWidth = 512;
    public int canvasHeight = 512;
    public Color backgroundColor = Color.white;

    [Header("Brush")]
    public Color brushColor = Color.black;
    public int brushSize = 8;

    private Texture2D canvasTexture;
    private SpriteRenderer spriteRenderer;
    private Color[] undoBuffer;
    private bool hasChange;

    /// <summary>
    /// Initializes the canvas texture and assigns it to the sprite renderer.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        canvasTexture = new Texture2D(canvasWidth, canvasHeight);
        ClearCanvas();
        spriteRenderer.sprite = Sprite.Create(canvasTexture, new Rect(0, 0, canvasWidth, canvasHeight), Vector2.one * 0.5f);
    }

    /// <summary>
    /// Fills the canvas with the background color.
    /// </summary>
    private void ClearCanvas()
    {
        Color[] pixels = new Color[canvasWidth * canvasHeight];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = backgroundColor;
        canvasTexture.SetPixels(pixels);
        canvasTexture.Apply();
    }

    /// <summary>
    /// Draws at the given world position using the current brush color and size.
    /// Converts world coordinates to texture coordinates and paints a circular brush stroke.
    /// </summary>
    public void DrawAt(Vector2 worldPos)
    {
        Vector2 localPos = worldPos - (Vector2)transform.position;
        Vector2 texSize = new Vector2(spriteRenderer.sprite.textureRect.width, spriteRenderer.sprite.textureRect.height);
        Vector2 scale = new Vector2(
            canvasWidth / (transform.localScale.x * texSize.x),
            canvasHeight / (transform.localScale.y * texSize.y)
        );
        Vector2Int texCoord = new Vector2Int(
            Mathf.RoundToInt(localPos.x * scale.x + canvasWidth / 2f),
            Mathf.RoundToInt(localPos.y * scale.y + canvasHeight / 2f)
        );

        if (texCoord.x < 0 || texCoord.x >= canvasWidth || texCoord.y < 0 || texCoord.y >= canvasHeight)
            return;

        // Save state for undo before making changes
        SaveUndo();
        for (int x = -brushSize / 2; x <= brushSize / 2; x++)
        {
            for (int y = -brushSize / 2; y <= brushSize / 2; y++)
            {
                int px = texCoord.x + x;
                int py = texCoord.y + y;
                if (px >= 0 && px < canvasWidth && py >= 0 && py < canvasHeight)
                {
                    float dist = Mathf.Sqrt(x * x + y * y);
                    // Apply pixel only if within circular brush radius
                    if (dist <= brushSize / 2f)
                        canvasTexture.SetPixel(px, py, brushColor);
                }
            }
        }
        canvasTexture.Apply();
        hasChange = true;
    }

    /// <summary>
    /// Clears the entire canvas, saving the current state for undo.
    /// </summary>
    public void ClearAll()
    {
        SaveUndo();
        ClearCanvas();
    }

    /// <summary>
    /// Reverts the canvas to the last saved undo buffer.
    /// </summary>
    public void Undo()
    {
        if (undoBuffer == null) return;
        canvasTexture.SetPixels(undoBuffer);
        canvasTexture.Apply();
        undoBuffer = null;
    }

    /// <summary>
    /// Saves the current canvas pixels into the undo buffer if no changes have been made yet.
    /// </summary>
    private void SaveUndo()
    {
        if (!hasChange)
            undoBuffer = canvasTexture.GetPixels();
    }

    /// <summary>
    /// Sets the active brush color.
    /// </summary>
    public void SetBrushColor(Color color)
    {
        brushColor = color;
    }

    /// <summary>
    /// Sets the brush size, clamped between 1 and 50.
    /// </summary>
    public void SetBrushSize(int size)
    {
        brushSize = Mathf.Clamp(size, 1, 50);
    }
}
