using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        placementController = FindAnyObjectByType<PlacementController>();
        arManager = FindAnyObjectByType<ARManager>();

        if (placementButton != null)
            placementButton.onClick.AddListener(OnPlacementButtonClicked);

        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetButtonClicked);

        UpdateStatus("AR ready. Tap to place objects.");
    }

    private void OnPlacementButtonClicked()
    {
        placementController.RemoveSelectedObject();
    }

    private void OnResetButtonClicked()
    {
        placementController.ClearAllObjects();
        arManager.ResetARSession();
        UpdateStatus("Session reset.");
    }

    public void UpdateStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    private void Update()
    {
        if (showDebugInfo && statusText != null)
        {
            statusText.text = $"FPS: {1f / Time.deltaTime:F0}";
        }
    }
}
