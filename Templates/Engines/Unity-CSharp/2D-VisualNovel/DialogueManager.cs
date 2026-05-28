using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string characterName;
        public string text;
        public Sprite characterSprite;
        public AudioClip voiceClip;
    }

    [System.Serializable]
    public class DialogueNode
    {
        public string nodeId;
        public DialogueLine[] lines;
        public Choice[] choices;
        public string nextNodeId;
        public string backgroundSceneId;
    }

    [System.Serializable]
    public class Choice
    {
        public string text;
        public string nextNodeId;
        public string[] requiredFlags;
        public string[] setFlags;
    }

    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public UnityEngine.UI.Image characterImage;
    public UnityEngine.UI.Image backgroundImage;
    public GameObject choicePanel;
    public UnityEngine.UI.Button[] choiceButtons;
    public GameObject continueIndicator;

    [Header("Settings")]
    public float textSpeed = 0.05f;
    public bool autoPlay;
    public float autoPlayDelay = 3f;

    private Dictionary<string, DialogueNode> dialogueNodes;
    private DialogueNode currentNode;
    private int currentLineIndex;
    private Coroutine textRoutine;
    private bool isTextRevealing;
    private HashSet<string> flags = new HashSet<string>();

    private void Awake()
    {
        dialogueNodes = new Dictionary<string, DialogueNode>();
    }

    public void LoadDialogueNode(string nodeId)
    {
        if (!dialogueNodes.ContainsKey(nodeId)) return;

        currentNode = dialogueNodes[nodeId];
        currentLineIndex = 0;
        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (currentNode == null || currentLineIndex >= currentNode.lines.Length)
        {
            ShowChoices();
            return;
        }

        DialogueLine line = currentNode.lines[currentLineIndex];
        if (nameText != null) nameText.text = line.characterName;
        if (characterImage != null) characterImage.sprite = line.characterSprite;

        if (textRoutine != null) StopCoroutine(textRoutine);
        textRoutine = StartCoroutine(RevealText(line.text));
    }

    private IEnumerator RevealText(string text)
    {
        isTextRevealing = true;
        dialogueText.text = "";
        if (continueIndicator != null) continueIndicator.SetActive(false);

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTextRevealing = false;
        if (continueIndicator != null) continueIndicator.SetActive(true);

        if (autoPlay)
        {
            yield return new WaitForSeconds(autoPlayDelay);
            AdvanceDialogue();
        }
    }

    public void AdvanceDialogue()
    {
        if (isTextRevealing)
        {
            if (textRoutine != null) StopCoroutine(textRoutine);
            dialogueText.text = currentNode.lines[currentLineIndex].text;
            isTextRevealing = false;
            if (continueIndicator != null) continueIndicator.SetActive(true);
            return;
        }

        currentLineIndex++;
        ShowCurrentLine();
    }

    private void ShowChoices()
    {
        if (currentNode == null) return;

        if (currentNode.choices == null || currentNode.choices.Length == 0)
        {
            if (!string.IsNullOrEmpty(currentNode.nextNodeId))
            {
                LoadDialogueNode(currentNode.nextNodeId);
            }
            return;
        }

        choicePanel.SetActive(true);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < currentNode.choices.Length)
            {
                Choice choice = currentNode.choices[i];
                bool available = true;

                if (choice.requiredFlags != null)
                {
                    foreach (string flag in choice.requiredFlags)
                    {
                        if (!flags.Contains(flag))
                        {
                            available = false;
                            break;
                        }
                    }
                }

                choiceButtons[i].gameObject.SetActive(available);
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
                int index = i;
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => SelectChoice(index));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void SelectChoice(int index)
    {
        if (currentNode.choices == null || index >= currentNode.choices.Length) return;

        Choice choice = currentNode.choices[index];
        if (choice.setFlags != null)
        {
            foreach (string flag in choice.setFlags)
                flags.Add(flag);
        }

        choicePanel.SetActive(false);

        if (!string.IsNullOrEmpty(choice.nextNodeId))
        {
            LoadDialogueNode(choice.nextNodeId);
        }
    }

    public void AddFlag(string flag) => flags.Add(flag);
    public bool HasFlag(string flag) => flags.Contains(flag);
    public void SetAutoPlay(bool value) => autoPlay = value;
}
