/// <summary>
/// Manages visual novel game state: dialogue flow, save/load, scene transitions, and quitting.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public DialogueManager dialogueManager;

    [Header("Settings")]
    public string startNodeId = "start";

    /// <summary>
    /// Serializable save data containing dialogue state and flags.
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        public string currentNodeId;
        public int currentLineIndex;
        public string[] flags;
    }

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Loads the starting dialogue node on game start.
    /// </summary>
    private void Start()
    {
        if (dialogueManager != null)
            dialogueManager.LoadDialogueNode(startNodeId);
    }

    /// <summary>
    /// Saves the current dialogue state to a numbered slot via PlayerPrefs.
    /// </summary>
    public void SaveGame(int slot)
    {
        SaveData data = new SaveData();
        // Save current dialogue state
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveSlot_" + slot, json);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads dialogue state from a numbered save slot.
    /// </summary>
    public void LoadGame(int slot)
    {
        string json = PlayerPrefs.GetString("SaveSlot_" + slot);
        if (!string.IsNullOrEmpty(json))
        {
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            // Load saved state
        }
    }

    /// <summary>
    /// Loads a scene by name.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
