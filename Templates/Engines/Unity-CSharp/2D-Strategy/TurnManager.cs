/// <summary>
/// Manages turn-based gameplay: alternates between player and enemy turns, with simple enemy AI.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public enum TurnPhase { Player, Enemy }
    public TurnPhase currentPhase;
    public int currentTurn = 1;

    [Header("References")]
    public GridManager gridManager;

    private List<Unit> playerUnits;
    private List<Unit> enemyUnits;

    /// <summary>
    /// Finds all units in the scene and starts the first player turn.
    /// </summary>
    private void Start()
    {
        playerUnits = new List<Unit>(FindObjectsByType<Unit>(FindObjectsSortMode.None));
        enemyUnits = new List<Unit>();
        currentPhase = TurnPhase.Player;
        StartPlayerTurn();
    }

    /// <summary>
    /// Resets all player units and grants income for the new turn.
    /// </summary>
    public void StartPlayerTurn()
    {
        currentPhase = TurnPhase.Player;
        foreach (Unit unit in playerUnits)
        {
            unit.ResetTurn();
        }
        GameManager.Instance.NextTurn();
    }

    /// <summary>
    /// Ends the player turn and begins the enemy turn.
    /// </summary>
    public void EndPlayerTurn()
    {
        currentPhase = TurnPhase.Enemy;
        StartEnemyTurn();
    }

    /// <summary>
    /// Executes enemy AI: each enemy moves toward the nearest player unit and attacks.
    /// </summary>
    private void StartEnemyTurn()
    {
        foreach (Unit unit in enemyUnits)
        {
            unit.ResetTurn();
            // Simple AI: move toward nearest player unit and attack
            if (playerUnits.Count > 0)
            {
                Unit nearest = FindNearest(playerUnits, unit);
                if (nearest != null)
                {
                    Vector2Int dir = nearest.GetGridPosition() - unit.GetGridPosition();
                    Vector2Int moveTarget = unit.GetGridPosition() + new Vector2Int(
                        Mathf.Clamp(dir.x, -1, 1),
                        Mathf.Clamp(dir.y, -1, 1)
                    );
                    unit.MoveTo(moveTarget);
                    unit.Attack(nearest);
                }
            }
        }

        currentTurn++;
        StartPlayerTurn();
    }

    /// <summary>
    /// Finds the nearest unit from a list to a source unit.
    /// </summary>
    private Unit FindNearest(List<Unit> units, Unit source)
    {
        Unit nearest = null;
        float minDist = float.MaxValue;
        Vector2Int sourcePos = source.GetGridPosition();

        foreach (Unit unit in units)
        {
            Vector2Int unitPos = unit.GetGridPosition();
            float dist = Vector2Int.Distance(sourcePos, unitPos);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = unit;
            }
        }
        return nearest;
    }

    /// <summary>
    /// Registers a player unit in the turn list.
    /// </summary>
    public void RegisterPlayerUnit(Unit unit)
    {
        if (!playerUnits.Contains(unit))
            playerUnits.Add(unit);
    }

    /// <summary>
    /// Registers an enemy unit in the turn list.
    /// </summary>
    public void RegisterEnemyUnit(Unit unit)
    {
        if (!enemyUnits.Contains(unit))
            enemyUnits.Add(unit);
    }

    /// <summary>
    /// Removes a unit from both player and enemy lists.
    /// </summary>
    public void RemoveUnit(Unit unit)
    {
        playerUnits.Remove(unit);
        enemyUnits.Remove(unit);
    }
}
