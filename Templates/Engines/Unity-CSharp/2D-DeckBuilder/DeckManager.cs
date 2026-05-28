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

    public static DeckManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        drawPile = new List<CardData>(allCards);
        Shuffle(drawPile);
    }

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

    public void DiscardCard(CardData card)
    {
        hand.Remove(card);
        discardPile.Add(card);
    }

    public void DiscardHand()
    {
        discardPile.AddRange(hand);
        hand.Clear();
    }
}
