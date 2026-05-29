using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Central game manager for the AR scene. Handles UI state and singleton lifecycle.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public Button placementButton;
    public Button resetButton;
    public TMPro.TextMeshProUGUI statusText;

    [Header("Debug")]
    public bool showDebugInfo = true;

    private PlacementController placementController;
    private ARManager arManager;

    private void Awake()
    {
        // Enforce singleton pattern
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        // Find required components and wire up UI buttons
        placementController = FindAnyObjectByType<PlacementController>();
        arManager = FindAnyObjectByType<ARManager>();

        if (placementButton != null)
            placementButton.onClick.AddListener(OnPlacementButtonClicked);

        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetButtonClicked);

        UpdateStatus("AR ready. Tap to place objects.");
    }

    /// <summary>
    /// Called when the placement button is clicked. Removes the selected object.
    /// </summary>
    private void OnPlacementButtonClicked()
    {
        placementController.RemoveSelectedObject();
    }

    /// <summary>
    /// Called when the reset button is clicked. Clears all objects and resets the AR session.
    /// </summary>
    private void OnResetButtonClicked()
    {
        placementController.ClearAllObjects();
        arManager.ResetARSession();
        UpdateStatus("Session reset.");
    }

    /// <summary>
    /// Updates the on-screen status text.
    /// </summary>
    public void UpdateStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    private void Update()
    {
        // Show debug FPS counter if enabled
        if (showDebugInfo && statusText != null)
        {
            statusText.text = $"FPS: {1f / Time.deltaTime:F0}";
        }
    }
}
