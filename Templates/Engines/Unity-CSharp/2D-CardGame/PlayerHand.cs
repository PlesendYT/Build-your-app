/// <summary>
/// Represents a player's hand in the card game, handling drawing, mana, health, and card visuals.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [Header("References")]
    public Deck deck;
    public GameObject cardPrefab;
    public Transform handLayout;
    public TMPro.TextMeshProUGUI deckCountText;
    public TMPro.TextMeshProUGUI handCountText;

    public int maxHandSize = 7;
    public int maxMana = 10;
    public int currentMana;
    public int health = 30;
    public int maxHealth = 30;

    private List<CardInstance> handCards = new List<CardInstance>();
    private GameManager gameManager;

    /// <summary>
    /// Finds the GameManager instance on awake.
    /// </summary>
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    /// <summary>
    /// Initializes mana to maximum on start.
    /// </summary>
    private void Start()
    {
        currentMana = maxMana;
    }

    /// <summary>
    /// Called at the start of each turn: increments mana and draws a card.
    /// </summary>
    public void StartTurn()
    {
        currentMana = Mathf.Min(currentMana + 1, maxMana);
        DrawCards(1);
        UpdateUI();
    }

    /// <summary>
    /// Draws cards from the deck up to the maximum hand size.
    /// </summary>
    public void DrawCards(int count)
    {
        for (int i = 0; i < count && handCards.Count < maxHandSize; i++)
        {
            CardInstance card = deck.DrawCard();
            if (card != null)
            {
                handCards.Add(card);
                CreateCardVisual(card);
            }
        }
        UpdateUI();
    }

    /// <summary>
    /// Applies damage to the player and triggers game over if health reaches zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        health = Mathf.Max(0, health - amount);
        if (health <= 0)
        {
            gameManager.GameOver();
        }
    }

    /// <summary>
    /// Heals the player up to the maximum health.
    /// </summary>
    public void Heal(int amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
    }

    /// <summary>
    /// Spends mana if enough is available; returns true on success.
    /// </summary>
    public bool SpendMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Instantiates a card visual game object in the hand layout.
    /// </summary>
    private void CreateCardVisual(CardInstance card)
    {
        if (cardPrefab == null || handLayout == null) return;
        GameObject obj = Instantiate(cardPrefab, handLayout);
    }

    /// <summary>
    /// Updates the deck and hand count text displays.
    /// </summary>
    public void UpdateUI()
    {
        if (deckCountText != null) deckCountText.text = "Deck: " + deck.GetDeckCount();
        if (handCountText != null) handCountText.text = "Hand: " + handCards.Count;
    }
}
