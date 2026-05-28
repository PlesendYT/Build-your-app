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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        canvasTexture = new Texture2D(canvasWidth, canvasHeight);
        ClearCanvas();
        spriteRenderer.sprite = Sprite.Create(canvasTexture, new Rect(0, 0, canvasWidth, canvasHeight), Vector2.one * 0.5f);
    }

    private void ClearCanvas()
    {
        Color[] pixels = new Color[canvasWidth * canvasHeight];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = backgroundColor;
        canvasTexture.SetPixels(pixels);
        canvasTexture.Apply();
    }

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
                    if (dist <= brushSize / 2f)
                        canvasTexture.SetPixel(px, py, brushColor);
                }
            }
        }
        canvasTexture.Apply();
        hasChange = true;
    }

    public void ClearAll()
    {
        SaveUndo();
        ClearCanvas();
    }

    public void Undo()
    {
        if (undoBuffer == null) return;
        canvasTexture.SetPixels(undoBuffer);
        canvasTexture.Apply();
        undoBuffer = null;
    }

    private void SaveUndo()
    {
        if (!hasChange)
            undoBuffer = canvasTexture.GetPixels();
    }

    public void SetBrushColor(Color color)
    {
        brushColor = color;
    }

    public void SetBrushSize(int size)
    {
        brushSize = Mathf.Clamp(size, 1, 50);
    }
}
