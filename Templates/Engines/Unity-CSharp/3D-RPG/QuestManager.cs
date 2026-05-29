/// <summary>
/// Quest system with active/completed tracking, logging, and completion events.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    public string description;
    public bool isCompleted;
    public int rewardGold;
}

public class QuestManager : MonoBehaviour
{
    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();

    public TMPro.TextMeshProUGUI questLogText;

    public event System.Action<Quest> OnQuestCompleted;

    public void StartQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            Debug.Log("Quest started: " + quest.questName);
        }
    }

    public void CompleteQuest(Quest quest)
    {
        if (activeQuests.Contains(quest))
        {
            quest.isCompleted = true;
            activeQuests.Remove(quest);
            completedQuests.Add(quest);
            OnQuestCompleted?.Invoke(quest);
            UpdateQuestLog();
        }
    }

    private void UpdateQuestLog()
    {
        if (questLogText != null)
        {
            questLogText.text = "Active Quests:\n";
            foreach (var q in activeQuests)
                questLogText.text += "- " + q.questName + "\n";
        }
    }
}

[System.Serializable]
public class QuestObjective
{
    public string description;
    public string target;
    public int requiredAmount;
    public int currentAmount;
    public bool IsComplete => currentAmount >= requiredAmount;
}

[System.Serializable]
public class Reward
{
    public int experience;
    public int gold;
    public Item[] items;
}

public class QuestManager : MonoBehaviour
{
    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();

    public void AcceptQuest(Quest quest)
    {
        quest.isActive = true;
        activeQuests.Add(quest);
    }

    public void UpdateObjective(string objectiveTarget, int progress = 1)
    {
        foreach (Quest quest in activeQuests)
        {
            foreach (QuestObjective obj in quest.objectives)
            {
                if (obj.target == objectiveTarget)
                {
                    obj.currentAmount += progress;
                    CheckQuestCompletion(quest);
                }
            }
        }
    }

    private void CheckQuestCompletion(Quest quest)
    {
        foreach (QuestObjective obj in quest.objectives)
        {
            if (!obj.IsComplete) return;
        }

        CompleteQuest(quest);
    }

    private void CompleteQuest(Quest quest)
    {
        quest.isActive = false;
        quest.isComplete = true;
        activeQuests.Remove(quest);
        completedQuests.Add(quest);

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.AddExperience(quest.reward.experience);
        }
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void GameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
