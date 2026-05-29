/// <summary>
/// Defines card data and manages draw pile, discard pile, and hand for a deck-builder game.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CardData
{
    public string cardName;
    public int cost;
    public int damage;
    public int block;
    public string description;
    public Sprite artwork;
}

public class DeckManager : MonoBehaviour
{
    [Header("Decks")]
    public List<CardData> allCards = new List<CardData>();
    public List<CardData> drawPile = new List<CardData>();
    public List<CardData> discardPile = new List<CardData>();
    public List<CardData> hand = new List<CardData>();
    public int handSize = 5;

    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static DeckManager Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Copies all cards into the draw pile and shuffles them.
    /// </summary>
    private void Start()
    {
        drawPile = new List<CardData>(allCards);
        Shuffle(drawPile);
    }

    /// <summary>
    /// Fisher-Yates shuffle for a list of cards.
    /// </summary>
    public void Shuffle(List<CardData> pile)
    {
        for (int i = pile.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            CardData temp = pile[i];
            pile[i] = pile[j];
            pile[j] = temp;
        }
    }

    /// <summary>
    /// Draws cards from the draw pile, reshuffling discard into draw pile if empty.
    /// </summary>
    public List<CardData> DrawCards(int count)
    {
        List<CardData> drawn = new List<CardData>();
        for (int i = 0; i < count; i++)
        {
            if (drawPile.Count == 0)
            {
                drawPile = new List<CardData>(discardPile);
                discardPile.Clear();
                Shuffle(drawPile);
            }

            if (drawPile.Count > 0)
            {
                CardData card = drawPile[0];
                drawPile.RemoveAt(0);
                hand.Add(card);
                drawn.Add(card);
            }
        }
        return drawn;
    }

    /// <summary>
    /// Moves a card from the hand to the discard pile.
    /// </summary>
    public void DiscardCard(CardData card)
    {
        hand.Remove(card);
        discardPile.Add(card);
    }

    /// <summary>
    /// Discards the entire hand into the discard pile.
    /// </summary>
    public void DiscardHand()
    {
        discardPile.AddRange(hand);
        hand.Clear();
    }
}
