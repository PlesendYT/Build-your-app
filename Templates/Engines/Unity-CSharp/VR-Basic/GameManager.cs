using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("VR Settings")]
    public float defaultPlayerHeight = 1.8f;
    public bool showDebugInfo = true;

    [Header("UI")]
    public TMPro.TextMeshProUGUI debugText;

    private VRInput vrInput;

    private void Awake()
    {
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
        if (showDebugInfo && debugText != null)
        {
            debugText.text = $"FPS: {1f / Time.deltaTime:F0}\n" +
                            $"Frame Time: {Time.deltaTime * 1000f:F1}ms";
        }
    }

    public void ToggleMovementMode()
    {
        if (vrInput != null)
            vrInput.ToggleMovementMode();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
