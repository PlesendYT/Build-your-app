using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public DialogueManager dialogueManager;

    [Header("Settings")]
    public string startNodeId = "start";

    [System.Serializable]
    public class SaveData
    {
        public string currentNodeId;
        public int currentLineIndex;
        public string[] flags;
    }

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

    private void Start()
    {
        if (dialogueManager != null)
            dialogueManager.LoadDialogueNode(startNodeId);
    }

    public void SaveGame(int slot)
    {
        SaveData data = new SaveData();
        // Save current dialogue state
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveSlot_" + slot, json);
        PlayerPrefs.Save();
    }

    public void LoadGame(int slot)
    {
        string json = PlayerPrefs.GetString("SaveSlot_" + slot);
        if (!string.IsNullOrEmpty(json))
        {
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            // Load saved state
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
