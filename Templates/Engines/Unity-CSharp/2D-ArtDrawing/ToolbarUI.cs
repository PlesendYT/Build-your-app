/// <summary>
/// Controls the drawing toolbar UI, allowing the user to select colors, adjust brush size, clear, and undo.
/// </summary>
using UnityEngine;
using TMPro;

public class ToolbarUI : MonoBehaviour
{
    public DrawingCanvas canvas;
    public TMP_Text brushSizeText;
    public TMP_Text colorText;
    public GameObject colorPickerPanel;

    /// <summary>
    /// Finds the drawing canvas if not assigned and updates the UI.
    /// </summary>
    private void Start()
    {
        if (canvas == null) canvas = FindObjectOfType<DrawingCanvas>();
        UpdateUI();
    }

    /// <summary>
    /// Sets the brush color to red.
    /// </summary>
    public void OnRedSelected()
    {
        canvas.SetBrushColor(Color.red);
        UpdateUI();
    }

    /// <summary>
    /// Sets the brush color to blue.
    /// </summary>
    public void OnBlueSelected()
    {
        canvas.SetBrushColor(Color.blue);
        UpdateUI();
    }

    /// <summary>
    /// Sets the brush color to green.
    /// </summary>
    public void OnGreenSelected()
    {
        canvas.SetBrushColor(Color.green);
        UpdateUI();
    }

    /// <summary>
    /// Sets the brush color to black.
    /// </summary>
    public void OnBlackSelected()
    {
        canvas.SetBrushColor(Color.black);
        UpdateUI();
    }

    /// <summary>
    /// Sets the brush color to white.
    /// </summary>
    public void OnWhiteSelected()
    {
        canvas.SetBrushColor(Color.white);
        UpdateUI();
    }

    /// <summary>
    /// Increases the brush size by 2.
    /// </summary>
    public void OnIncreaseBrush()
    {
        canvas.SetBrushSize(canvas.brushSize + 2);
        UpdateUI();
    }

    /// <summary>
    /// Decreases the brush size by 2.
    /// </summary>
    public void OnDecreaseBrush()
    {
        canvas.SetBrushSize(canvas.brushSize - 2);
        UpdateUI();
    }

    /// <summary>
    /// Clears the entire canvas.
    /// </summary>
    public void OnClear()
    {
        canvas.ClearAll();
    }

    /// <summary>
    /// Reverts the last drawing action.
    /// </summary>
    public void OnUndo()
    {
        canvas.Undo();
    }

    /// <summary>
    /// Refreshes the displayed brush size text.
    /// </summary>
    private void UpdateUI()
    {
        if (brushSizeText != null)
            brushSizeText.text = "Brush: " + canvas.brushSize;
    }
}
