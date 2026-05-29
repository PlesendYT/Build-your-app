using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Central game manager for the VR scene. Handles debug info and movement mode toggling.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("VR Settings")]
    public float defaultPlayerHeight = 1.8f;
    public bool showDebugInfo = true;

    [Header("UI")]
    public TMPro.TextMeshProUGUI debugText;

    private VRInput vrInput;

    private void Awake()
    {
        // Enforce singleton pattern
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        vrInput = FindAnyObjectByType<VRInput>();
        Application.targetFrameRate = 90;
    }

    private void Update()
    {
        // Display FPS and frame time debug info if enabled
        if (showDebugInfo && debugText != null)
        {
            debugText.text = $"FPS: {1f / Time.deltaTime:F0}\n" +
                            $"Frame Time: {Time.deltaTime * 1000f:F1}ms";
        }
    }

    /// <summary>
    /// Toggles between teleport and continuous movement modes.
    /// </summary>
    public void ToggleMovementMode()
    {
        if (vrInput != null)
            vrInput.ToggleMovementMode();
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();
    }
}
