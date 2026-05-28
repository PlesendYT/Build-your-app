using UnityEngine;
using TMPro;

public class ToolbarUI : MonoBehaviour
{
    public DrawingCanvas canvas;
    public TMP_Text brushSizeText;
    public TMP_Text colorText;
    public GameObject colorPickerPanel;

    private void Start()
    {
        if (canvas == null) canvas = FindObjectOfType<DrawingCanvas>();
        UpdateUI();
    }

    public void OnRedSelected()
    {
        canvas.SetBrushColor(Color.red);
        UpdateUI();
    }

    public void OnBlueSelected()
    {
        canvas.SetBrushColor(Color.blue);
        UpdateUI();
    }

    public void OnGreenSelected()
    {
        canvas.SetBrushColor(Color.green);
        UpdateUI();
    }

    public void OnBlackSelected()
    {
        canvas.SetBrushColor(Color.black);
        UpdateUI();
    }

    public void OnWhiteSelected()
    {
        canvas.SetBrushColor(Color.white);
        UpdateUI();
    }

    public void OnIncreaseBrush()
    {
        canvas.SetBrushSize(canvas.brushSize + 2);
        UpdateUI();
    }

    public void OnDecreaseBrush()
    {
        canvas.SetBrushSize(canvas.brushSize - 2);
        UpdateUI();
    }

    public void OnClear()
    {
        canvas.ClearAll();
    }

    public void OnUndo()
    {
        canvas.Undo();
    }

    private void UpdateUI()
    {
        if (brushSizeText != null)
            brushSizeText.text = "Brush: " + canvas.brushSize;
    }
}
