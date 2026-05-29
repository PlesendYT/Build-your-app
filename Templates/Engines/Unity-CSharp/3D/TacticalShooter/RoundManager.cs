/// <summary>
/// Attack/defend round system with bomb plant/defuse, spawn points, and buy phase.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour
{
    [Header("Objectives")]
    public Transform bombSiteA;
    public Transform bombSiteB;
    public bool isBombPlanted;
    public float bombTimer = 45f;
    public float plantTime = 3f;
    public float defuseTime = 5f;

    [Header("Spawns")]
    public Transform[] attackerSpawns;
    public Transform[] defenderSpawns;

    [Header("Buy Phase")]
    public bool isBuyPhase;
    public float buyPhaseDuration = 30f;
    public float buyTimer;

    private void Start()
    {
        StartBuyPhase();
    }

    public void StartBuyPhase()
    {
        isBuyPhase = true;
        buyTimer = buyPhaseDuration;
        isBombPlanted = false;

        // Teleport players to spawns
        TeleportPlayers();
    }

    private void TeleportPlayers()
    {
        // Assign attackers to their spawns
        for (int i = 0; i < GameManager.Instance.attackers.Count; i++)
        {
            if (i < attackerSpawns.Length && GameManager.Instance.attackers[i] != null)
            {
                GameManager.Instance.attackers[i].transform.position = attackerSpawns[i].position;
            }
        }

        // Assign defenders to their spawns
        for (int i = 0; i < GameManager.Instance.defenders.Count; i++)
        {
            if (i < defenderSpawns.Length && GameManager.Instance.defenders[i] != null)
            {
                GameManager.Instance.defenders[i].transform.position = defenderSpawns[i].position;
            }
        }
    }

    private void Update()
    {
        if (isBuyPhase)
        {
            buyTimer -= Time.deltaTime;
            if (buyTimer <= 0f)
            {
                isBuyPhase = false;
                GameManager.Instance.StartNewRound();
            }
        }

        if (isBombPlanted)
        {
            bombTimer -= Time.deltaTime;
            if (bombTimer <= 0f)
            {
                BombDetonated();
            }
        }
    }

    public void PlantBomb(Transform plantLocation)
    {
        if (isBombPlanted) return;
        isBombPlanted = true;
        bombTimer = 45f;
    }

    public void DefuseBomb()
    {
        if (!isBombPlanted) return;
        isBombPlanted = false;

        // Defenders win the round
        GameManager.Instance.EndRound(Team.Defenders);
    }

    private void BombDetonated()
    {
        isBombPlanted = false;

        // Attackers win the round
        GameManager.Instance.EndRound(Team.Attackers);
    }

    public bool IsAtBombSite(Vector3 position, float range = 3f)
    {
        if (bombSiteA != null && Vector3.Distance(position, bombSiteA.position) <= range)
            return true;
        if (bombSiteB != null && Vector3.Distance(position, bombSiteB.position) <= range)
            return true;
        return false;
    }
}
