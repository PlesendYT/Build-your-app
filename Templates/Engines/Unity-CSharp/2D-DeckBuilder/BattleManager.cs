using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [Header("Player")]
    public int playerHealth = 80;
    public int playerMaxHealth = 80;
    public int playerEnergy = 3;
    public int playerMaxEnergy = 3;

    [Header("Enemy")]
    public int enemyHealth = 50;
    public int enemyMaxHealth = 50;
    public int enemyDamage = 6;
    public string enemyName = "Goblin";

    [Header("UI")]
    public TMP_Text playerHPText;
    public TMP_Text playerEnergyText;
    public TMP_Text enemyHPText;
    public TMP_Text enemyNameText;
    public TMP_Text logText;
    public GameObject cardPrefab;
    public Transform handTransform;

    private int playerBlock;
    private int enemyBlock;
    private bool isPlayerTurn = true;

    private void Start()
    {
        DeckManager.Instance.DrawCards(DeckManager.Instance.handSize);
        StartPlayerTurn();
        UpdateUI();
    }

    private void StartPlayerTurn()
    {
        isPlayerTurn = true;
        playerEnergy = playerMaxEnergy;
        playerBlock = 0;
        DeckManager.Instance.DrawCards(5 - DeckManager.Instance.handSize);
        UpdateUI();
        LogMessage("Your turn!");
    }

    public void PlayCard(CardData card, bool targetEnemy = true)
    {
        if (!isPlayerTurn) return;
        if (playerEnergy < card.cost) return;

        playerEnergy -= card.cost;
        playerBlock += card.block;

        if (targetEnemy)
        {
            int damage = card.damage;
            if (enemyBlock > 0)
            {
                int blocked = Mathf.Min(enemyBlock, damage);
                enemyBlock -= blocked;
                damage -= blocked;
            }
            enemyHealth -= damage;
            LogMessage("Played " + card.cardName + " for " + card.damage + " damage!");
        }

        DeckManager.Instance.DiscardCard(card);
        UpdateUI();

        if (enemyHealth <= 0)
        {
            LogMessage("Victory! " + enemyName + " defeated!");
            isPlayerTurn = false;
        }
    }

    public void EndTurn()
    {
        if (!isPlayerTurn) return;
        isPlayerTurn = false;
        DeckManager.Instance.DiscardHand();
        StartCoroutine(EnemyTurn());
    }

    private System.Collections.IEnumerator EnemyTurn()
    {
        LogMessage(enemyName + "'s turn!");
        yield return new WaitForSeconds(1f);

        int damage = enemyDamage;
        if (playerBlock > 0)
        {
            int blocked = Mathf.Min(playerBlock, damage);
            playerBlock -= blocked;
            damage -= blocked;
        }
        playerHealth -= damage;
        enemyBlock = Random.Range(3, 8);
        UpdateUI();
        LogMessage(enemyName + " attacks for " + damage + " damage!");

        yield return new WaitForSeconds(1f);

        if (playerHealth <= 0)
        {
            LogMessage("Defeated! Game Over!");
        }
        else
        {
            StartPlayerTurn();
        }
    }

    private void LogMessage(string msg)
    {
        if (logText != null) logText.text = msg;
    }

    private void UpdateUI()
    {
        if (playerHPText != null) playerHPText.text = "HP: " + playerHealth + "/" + playerMaxHealth + (playerBlock > 0 ? " (+" + playerBlock + ")" : "");
        if (playerEnergyText != null) playerEnergyText.text = "Energy: " + playerEnergy + "/" + playerMaxEnergy;
        if (enemyHPText != null) enemyHPText.text = enemyName + ": " + Mathf.Max(0, enemyHealth) + "/" + enemyMaxHealth + (enemyBlock > 0 ? " (+" + enemyBlock + ")" : "");
        if (enemyNameText != null) enemyNameText.text = enemyName;
    }
}
