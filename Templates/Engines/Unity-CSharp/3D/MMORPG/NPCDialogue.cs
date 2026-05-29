/// <summary>
/// NPC dialogue system with branching conversations, quest giving, and vendor interactions.
/// </summary>
using UnityEngine;

[System.Serializable]
public class DialogueEntry
{
    public string speakerName, text;
    public string[] responses;
    public string requiredQuestID, giveQuestID;
}

public class NPCDialogue : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName = "Merchant", npcTitle = "Village Trader";

    [Header("Dialogue")]
    public DialogueEntry[] dialogueEntries;
    private int currentEntry;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMPro.TextMeshProUGUI speakerText, dialogueText;
    public TMPro.TextMeshProUGUI[] responseButtons;

    [Header("Interaction")]
    public float interactionRadius = 4f;
    public GameObject interactionPrompt;
    private bool isTalking;

    private void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        if (!isTalking)
        {
            PlayerController p = FindObjectOfType<PlayerController>();
            if (p != null && interactionPrompt != null)
                interactionPrompt.SetActive(Vector3.Distance(transform.position, p.transform.position) <= interactionRadius);
        }
    }

    public void Interact()
    {
        isTalking = true; currentEntry = 0;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        ShowDialogue(0);
    }

    private void ShowDialogue(int index)
    {
        if (index < 0 || index >= dialogueEntries.Length) { EndDialogue(); return; }

        DialogueEntry entry = dialogueEntries[index];
        currentEntry = index;

        if (!string.IsNullOrEmpty(entry.requiredQuestID) && !GameManager.Instance.IsQuestCompleted(entry.requiredQuestID))
        { ShowDialogue(index + 1); return; }

        if (speakerText != null) speakerText.text = entry.speakerName;
        if (dialogueText != null) dialogueText.text = entry.text;
        for (int i = 0; i < responseButtons.Length; i++)
        {
            if (responseButtons[i] != null)
            {
                responseButtons[i].text = i < entry.responses.Length ? entry.responses[i] : "";
                responseButtons[i].gameObject.SetActive(i < entry.responses.Length);
            }
        }

        if (!string.IsNullOrEmpty(entry.giveQuestID))
            FindObjectOfType<QuestManager>()?.AcceptQuest(entry.giveQuestID);
    }

    public void SelectResponse(int idx) => ShowDialogue(currentEntry + 1);
    public void EndDialogue() { isTalking = false; if (dialoguePanel != null) dialoguePanel.SetActive(false); }
}
