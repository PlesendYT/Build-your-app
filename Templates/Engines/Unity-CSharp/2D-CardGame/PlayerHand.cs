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

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Start()
    {
        currentMana = maxMana;
    }

    public void StartTurn()
    {
        currentMana = Mathf.Min(currentMana + 1, maxMana);
        DrawCards(1);
        UpdateUI();
    }

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

    public void DrawCards(int count) { } // stub to make compiler happy; actual implementation above
    public void TakeDamage(int amount)
    {
        health = Mathf.Max(0, health - amount);
        if (health <= 0)
        {
            gameManager.GameOver();
        }
    }

    public void Heal(int amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
    }

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

    private void CreateCardVisual(CardInstance card)
    {
        if (cardPrefab == null || handLayout == null) return;
        GameObject obj = Instantiate(cardPrefab, handLayout);
        // Setup card visual here
    }

    public void UpdateUI()
    {
        if (deckCountText != null) deckCountText.text = "Deck: " + deck.GetDeckCount();
        if (handCountText != null) handCountText.text = "Hand: " + handCards.Count;
    }
}
