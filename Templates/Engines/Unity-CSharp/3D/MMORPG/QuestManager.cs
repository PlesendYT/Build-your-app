/// <summary>
/// Quest tracking system: quest states, objectives, rewards, and progression management.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class QuestObjective
{
    public string description, targetID;
    public int requiredAmount, currentAmount;
    public bool isComplete;
    public void AddProgress(int amt = 1) { currentAmount += amt; if (currentAmount >= requiredAmount) isComplete = true; }
}

[System.Serializable]
public class Quest
{
    public string questID, questName, description;
    public List<QuestObjective> objectives = new List<QuestObjective>();
    public int goldReward, expReward;
    public string itemReward;
    public bool isActive, isComplete;
    public bool AllObjectivesComplete() { foreach (QuestObjective o in objectives) if (!o.isComplete) return false; return true; }
}

public class QuestManager : MonoBehaviour
{
    public List<Quest> availableQuests = new List<Quest>();
    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();

    [Header("UI")]
    public GameObject questLogPanel;
    public TMPro.TextMeshProUGUI questLogText;

    public void AcceptQuest(string id)
    {
        Quest q = availableQuests.Find(x => x.questID == id);
        if (q == null || q.isActive) return;
        q.isActive = true; availableQuests.Remove(q); activeQuests.Add(q);
    }

    public void UpdateQuestProgress(string targetID, int amount = 1)
    {
        foreach (Quest q in activeQuests)
        {
            foreach (QuestObjective o in q.objectives)
                if (o.targetID == targetID) o.AddProgress(amount);
            if (q.AllObjectivesComplete()) CompleteQuest(q);
        }
    }

    private void CompleteQuest(Quest q)
    {
        q.isComplete = true; q.isActive = false;
        GameManager.Instance.AddGold(q.goldReward);
        GameManager.Instance.AddExperience(q.expReward);
        GameManager.Instance.CompleteQuest(q.questID);
        activeQuests.Remove(q); completedQuests.Add(q);
    }

    public void ShowQuestLog()
    {
        if (questLogPanel != null) questLogPanel.SetActive(!questLogPanel.activeSelf);
        if (questLogText == null) return;
        string log = "";
        foreach (Quest q in activeQuests)
        {
            log += "<b>" + q.questName + "</b>\n" + q.description + "\n";
            foreach (QuestObjective o in q.objectives)
                log += "  - " + o.description + " (" + o.currentAmount + "/" + o.requiredAmount + ")\n";
            log += "\n";
        }
        questLogText.text = log;
    }
}
