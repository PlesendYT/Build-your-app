/// <summary>
/// Manages the creature collector game: overworld exploration, wild encounters, capture progress, and party.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Player Party")]
    public List<Creature> party = new List<Creature>();
    public int maxPartySize = 6;

    [Header("Collection")]
    public List<Creature> capturedCreatures = new List<Creature>();
    public int totalCreatureCount;

    [Header("Encounter")]
    public Creature[] wildCreaturePool;
    public float encounterRate = 0.1f;

    /// <summary>
    /// Initializes the player's starter creature.
    /// </summary>
    private void Start()
    {
        if (party.Count == 0)
        {
            Debug.Log("No party — choose a starter creature!");
        }
    }

    /// <summary>
    /// Random encounter check while the player moves in tall grass.
    /// </summary>
    public bool CheckRandomEncounter()
    {
        if (Random.value < encounterRate)
        {
            TriggerWildEncounter();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Starts a wild encounter with a random creature from the pool.
    /// </summary>
    private void TriggerWildEncounter()
    {
        if (wildCreaturePool.Length == 0) return;

        Creature wild = wildCreaturePool[Random.Range(0, wildCreaturePool.Length)];
        Creature instance = Instantiate(wild);
        Debug.Log("Wild " + instance.creatureName + " appeared! (Level " + instance.level + ")");
    }

    /// <summary>
    /// Attempts to capture a wild creature with a given capture power.
    /// </summary>
    public bool AttemptCapture(Creature target, float capturePower)
    {
        float captureChance = 1f - (float)target.currentHealth / target.maxHealth;
        captureChance = Mathf.Clamp01(captureChance + capturePower * 0.1f);

        if (Random.value < captureChance)
        {
            AddToCollection(target);
            Debug.Log("Caught " + target.creatureName + "!");
            return true;
        }

        Debug.Log(target.creatureName + " broke free!");
        return false;
    }

    /// <summary>
    /// Adds a creature to the captured collection and optionally to the party.
    /// </summary>
    public void AddToCollection(Creature creature)
    {
        capturedCreatures.Add(creature);
        if (party.Count < maxPartySize)
            party.Add(creature);
    }

    /// <summary>
    /// Heals all creatures in the party to full health.
    /// </summary>
    public void HealParty()
    {
        foreach (Creature c in party)
        {
            c.currentHealth = c.maxHealth;
        }
        Debug.Log("Party fully healed!");
    }
}
