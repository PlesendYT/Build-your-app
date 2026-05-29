using System.Collections.Generic;
using UnityEngine;

/// <summary>NPC dialogue system with branching dialogue trees and player responses.</summary>
public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private string _npcName = "Merchant";

    // Dialogue tree: key = node ID, value = {speaker text, responses}
    private Dictionary<string, DialogueNode> _dialogueTree;
    private string _currentNodeId;

    /// <summary>Structure for a single dialogue tree node.</summary>
    private struct DialogueNode
    {
        public string NpcLine;
        public List<ResponseOption> Responses;

        public DialogueNode(string line, List<ResponseOption> responses)
        {
            NpcLine = line;
            Responses = responses;
        }
    }

    /// <summary>Structure for a player response option.</summary>
    private struct ResponseOption
    {
        public string Text;
        public string NextNodeId;

        public ResponseOption(string text, string nextNodeId)
        {
            Text = text;
            NextNodeId = nextNodeId;
        }
    }

    /// <summary>Build the dialogue tree on start.</summary>
    private void Start()
    {
        _dialogueTree = new Dictionary<string, DialogueNode>
        {
            ["greeting"] = new DialogueNode(
                "Welcome, traveler! Need some supplies?",
                new List<ResponseOption>
                {
                    new ResponseOption("What are you selling?", "trade"),
                    new ResponseOption("Not right now.", "farewell"),
                }
            ),
            ["trade"] = new DialogueNode(
                "I've got potions, weapons, and rare artifacts.",
                new List<ResponseOption>
                {
                    new ResponseOption("Show me potions.", "potions"),
                    new ResponseOption("Maybe later.", "greeting"),
                }
            ),
            ["potions"] = new DialogueNode(
                "Health potions for 10 gold, mana for 8 gold.",
                new List<ResponseOption>
                {
                    new ResponseOption("I'll take one of each.", "farewell"),
                    new ResponseOption("Too expensive!", "greeting"),
                }
            ),
            ["farewell"] = new DialogueNode(
                "Safe travels, adventurer!",
                new List<ResponseOption>
                {
                    new ResponseOption("Goodbye.", "end"),
                }
            ),
            ["end"] = new DialogueNode("", new List<ResponseOption>()),
        };

        _currentNodeId = "greeting";
    }

    /// <summary>Called when the player interacts with this NPC.</summary>
    public void StartDialogue()
    {
        _currentNodeId = "greeting";
        DisplayCurrentNode();
    }

    /// <summary>Show the current NPC line and available responses.</summary>
    private void DisplayCurrentNode()
    {
        if (!_dialogueTree.TryGetValue(_currentNodeId, out DialogueNode node))
            return;

        Debug.Log($"[{_npcName}] {node.NpcLine}");

        for (int i = 0; i < node.Responses.Count; i++)
        {
            Debug.Log($"{i + 1}. {node.Responses[i].Text}");
        }
    }

    /// <summary>Select a response by index (0-based) to advance the dialogue.</summary>
    public void SelectResponse(int index)
    {
        if (!_dialogueTree.TryGetValue(_currentNodeId, out DialogueNode node))
            return;

        if (index < 0 || index >= node.Responses.Count)
            return;

        _currentNodeId = node.Responses[index].NextNodeId;

        if (_currentNodeId == "end")
        {
            Debug.Log("[Dialogue ended]");
            return;
        }

        DisplayCurrentNode();
    }

    /// <summary>Unity trigger for player interaction.</summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartDialogue();
        }
    }
}
