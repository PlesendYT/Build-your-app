/// <summary>
/// Manages the deck, draw pile, discard pile, and hand for a card game with shuffle and draw logic.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [Header("Starting Deck")]
    public Card[] startingCards;

    private Queue<CardInstance> drawPile;
    private List<CardInstance> discardPile;
    private List<CardInstance> hand;
    private List<CardInstance> exilePile;

    /// <summary>
    /// Initializes all card piles on awake.
    /// </summary>
    private void Awake()
    {
        drawPile = new Queue<CardInstance>();
        discardPile = new List<CardInstance>();
        hand = new List<CardInstance>();
        exilePile = new List<CardInstance>();
    }

    /// <summary>
    /// Builds and shuffles the starting deck.
    /// </summary>
    private void Start()
    {
        InitializeDeck();
    }

    /// <summary>
    /// Creates CardInstances from the starting cards, shuffles them, and populates the draw pile.
    /// </summary>
    public void InitializeDeck()
    {
        drawPile.Clear();
        discardPile.Clear();
        hand.Clear();

        List<CardInstance> cards = new List<CardInstance>();
        foreach (Card card in startingCards)
        {
            cards.Add(new CardInstance(card));
        }

        Shuffle(cards);
        foreach (CardInstance card in cards)
        {
            drawPile.Enqueue(card);
        }
    }

    /// <summary>
    /// Draws the top card from the draw pile, reshuffling the discard pile if needed.
    /// </summary>
    public CardInstance DrawCard()
    {
        if (drawPile.Count == 0)
        {
            ReshuffleDiscardPile();
            if (drawPile.Count == 0) return null;
        }

        CardInstance card = drawPile.Dequeue();
        hand.Add(card);
        return card;
    }

    /// <summary>
    /// Draws multiple cards from the draw pile.
    /// </summary>
    public void DrawCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            DrawCard();
        }
    }

    /// <summary>
    /// Moves a card from hand to the discard pile.
    /// </summary>
    public void DiscardCard(CardInstance card)
    {
        if (hand.Remove(card))
        {
            discardPile.Add(card);
        }
    }

    /// <summary>
    /// Discards all cards currently in hand.
    /// </summary>
    public void DiscardHand()
    {
        foreach (CardInstance card in hand)
        {
            discardPile.Add(card);
        }
        hand.Clear();
    }

    /// <summary>
    /// Moves a played card from hand to the discard pile.
    /// </summary>
    public void PlayCard(CardInstance card)
    {
        if (hand.Remove(card))
        {
            discardPile.Add(card);
        }
    }

    public int GetHandCount() => hand.Count;
    public int GetDeckCount() => drawPile.Count;
    public int GetDiscardCount() => discardPile.Count;

    /// <summary>
    /// Shuffles the discard pile back into the draw pile.
    /// </summary>
    private void ReshuffleDiscardPile()
    {
        if (discardPile.Count == 0) return;

        List<CardInstance> cards = new List<CardInstance>(discardPile);
        discardPile.Clear();
        Shuffle(cards);
        foreach (CardInstance card in cards)
        {
            drawPile.Enqueue(card);
        }
    }

    /// <summary>
    /// Fisher-Yates shuffle algorithm for a generic list.
    /// </summary>
    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
