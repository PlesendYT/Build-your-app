/// <summary>
/// Manages auto-battler rounds: board placement, combat resolution, and round progression.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    [Header("Board")]
    public List<Unit> playerUnits = new List<Unit>();
    public List<Unit> enemyUnits = new List<Unit>();
    public int boardWidth = 7;

    [Header("Combat")]
    public float roundTimer = 30f;
    private float currentRoundTime;
    public int currentRound = 1;
    public int playerHealth = 100;
    public int enemyHealth = 100;

    /// <summary>
    /// Starts the preparation phase for the first round.
    /// </summary>
    private void Start()
    {
        currentRoundTime = roundTimer;
        Debug.Log("Round " + currentRound + " — Place your units!");
    }

    /// <summary>
    /// Counts down the preparation timer and triggers combat when it reaches zero.
    /// </summary>
    private void Update()
    {
        currentRoundTime -= Time.deltaTime;
        if (currentRoundTime <= 0)
        {
            currentRoundTime = roundTimer;
            ResolveCombat();
        }
    }

    /// <summary>
    /// Places a unit on the player's board at the specified position.
    /// </summary>
    public bool PlaceUnit(Unit unit, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= boardWidth)
            return false;

        unit.boardPosition = slotIndex;
        playerUnits.Add(unit);
        Debug.Log(unit.unitName + " placed at slot " + slotIndex);
        return true;
    }

    /// <summary>
    /// Simulates auto-combat between player and enemy units, then advances the round.
    /// </summary>
    private void ResolveCombat()
    {
        Debug.Log("--- Combat Round " + currentRound + " ---");

        // Each unit attacks the nearest enemy
        for (int i = 0; i < playerUnits.Count && i < enemyUnits.Count; i++)
        {
            enemyUnits[i].TakeDamage(playerUnits[i].attackPower);
            playerUnits[i].TakeDamage(enemyUnits[i].attackPower);
        }

        // Remove defeated units
        playerUnits.RemoveAll(u => u.currentHealth <= 0);
        enemyUnits.RemoveAll(u => u.currentHealth <= 0);

        // Check win/loss condition
        if (enemyUnits.Count == 0)
        {
            enemyHealth -= 10;
            Debug.Log("Round won! Enemy base health: " + enemyHealth);
        }
        else if (playerUnits.Count == 0)
        {
            playerHealth -= 10;
            Debug.Log("Round lost! Player base health: " + playerHealth);
        }

        currentRound++;
        currentRoundTime = roundTimer;
        Debug.Log("Round " + currentRound + " starting. Place your units!");
    }

    /// <summary>
    /// Checks if the game is over by either side's health reaching zero.
    /// </summary>
    public bool IsGameOver()
    {
        return playerHealth <= 0 || enemyHealth <= 0;
    }
}
