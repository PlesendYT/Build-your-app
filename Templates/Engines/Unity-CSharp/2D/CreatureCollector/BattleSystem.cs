/// <summary>
/// Handles turn-based battles between the player's creatures and a wild or enemy creature.
/// </summary>
using UnityEngine;
using System.Collections;

public class BattleSystem : MonoBehaviour
{
    [Header("State")]
    public enum BattleState { Start, PlayerTurn, EnemyTurn, Won, Lost }
    public BattleState state;

    [Header("Participants")]
    public Creature playerCreature;
    public Creature enemyCreature;
    public GameManager gameManager;

    private void Start() { gameManager = FindObjectOfType<GameManager>(); }

    /// <summary>
    /// Begins the battle encounter with the given enemy creature.
    /// </summary>
    public void StartBattle(Creature player, Creature enemy)
    {
        playerCreature = player;
        enemyCreature = enemy;
        state = BattleState.PlayerTurn;
        Debug.Log("Battle started: " + playerCreature.creatureName + " vs " + enemyCreature.creatureName);
    }

    /// <summary>
    /// Executes the player's selected move against the enemy creature.
    /// </summary>
    public void PlayerAttack(int moveIndex)
    {
        if (state != BattleState.PlayerTurn) return;
        if (moveIndex < 0 || moveIndex >= playerCreature.learnedMoves.Count) return;

        Creature.Move move = playerCreature.learnedMoves[moveIndex];
        int damage = playerCreature.CalculateDamage(enemyCreature, move);
        enemyCreature.TakeDamage(damage);
        Debug.Log(playerCreature.creatureName + " used " + move.moveName + " for " + damage + " damage!");

        if (enemyCreature.currentHealth <= 0)
        {
            state = BattleState.Won;
            int xp = Mathf.FloorToInt(enemyCreature.level * 10f);
            Debug.Log("Enemy defeated! Gained " + xp + " XP.");
            return;
        }

        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    /// <summary>
    /// Runs the enemy's attack logic after a delay, then returns to the player's turn.
    /// </summary>
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        if (enemyCreature.learnedMoves.Count > 0)
        {
            Creature.Move move = enemyCreature.learnedMoves[0];
            int damage = enemyCreature.CalculateDamage(playerCreature, move);
            playerCreature.TakeDamage(damage);
            Debug.Log(enemyCreature.creatureName + " used " + move.moveName + " for " + damage + " damage!");
        }
        if (playerCreature.currentHealth <= 0) { state = BattleState.Lost; yield break; }
        state = BattleState.PlayerTurn;
    }

    /// <summary>
    /// Attempts to capture the enemy creature.
    /// </summary>
    public void AttemptCapture()
    {
        if (state != BattleState.PlayerTurn) return;
        bool caught = gameManager.AttemptCapture(enemyCreature, 1f);
        if (caught) state = BattleState.Won;
        else { state = BattleState.EnemyTurn; StartCoroutine(EnemyTurn()); }
    }

    /// <summary>
    /// Switches the active player creature to another from the party.
    /// </summary>
    public void SwitchCreature(Creature newCreature)
    {
        if (state != BattleState.PlayerTurn) return;
        playerCreature = newCreature;
        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }
}
