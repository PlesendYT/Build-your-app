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

    private void Start()
    {
        playerUnits = new List<Unit>(FindObjectsByType<Unit>(FindObjectsSortMode.None));
        enemyUnits = new List<Unit>();
        currentPhase = TurnPhase.Player;
        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        currentPhase = TurnPhase.Player;
        foreach (Unit unit in playerUnits)
        {
            unit.ResetTurn();
        }
        GameManager.Instance.NextTurn();
    }

    public void EndPlayerTurn()
    {
        currentPhase = TurnPhase.Enemy;
        StartEnemyTurn();
    }

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

    public void RegisterPlayerUnit(Unit unit)
    {
        if (!playerUnits.Contains(unit))
            playerUnits.Add(unit);
    }

    public void RegisterEnemyUnit(Unit unit)
    {
        if (!enemyUnits.Contains(unit))
            enemyUnits.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        playerUnits.Remove(unit);
        enemyUnits.Remove(unit);
    }
}
