// File: CardGameManager.cs
// Core game manager for a turn-based 2D card game. Handles deck, hand, mana, health, and card effects.

using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages the card game state including deck shuffling, hand management, mana, health,
/// card playing, and turn flow.
/// </summary>
public partial class CardGameManager : Node
{
    [Export] public int MaxHandSize = 7;
    [Export] public int MaxMana = 3;

    private List<Card> _drawPile = new();
    private List<Card> _hand = new();
    private List<Card> _discardPile = new();
    private Card[] _startingDeck;

    public int CurrentMana { get; private set; }
    public int CurrentHealth { get; private set; } = 30;
    public int MaxHealth { get; private set; } = 30;
    public int Shield { get; private set; } = 0;

    // Signals emitted when game state changes
    public Action<List<Card>> HandUpdated;
    public Action<int, int> ManaUpdated;
    public Action<int, int> HealthUpdated;
    public Action<bool> CombatEnd;

    public override void _Ready()
    {
        // Build the starting deck with basic cards
        _startingDeck = new Card[]
        {
            new Card { CardName = "Strike", Cost = 1, Damage = 6, CardType = "attack" },
            new Card { CardName = "Defend", Cost = 1, Shield = 5, CardType = "skill" },
            new Card { CardName = "Heal", Cost = 2, Heal = 8, CardType = "skill" }
        };
        ShuffleDeck();
    }

    /// <summary>
    /// Fisher-Yates shuffle of the draw pile. Clears hand and discard pile.
    /// </summary>
    public void ShuffleDeck()
    {
        _drawPile = new List<Card>(_startingDeck);
        var rng = new Random();
        for (int i = _drawPile.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            var temp = _drawPile[i];
            _drawPile[i] = _drawPile[j];
            _drawPile[j] = temp;
        }
        _hand.Clear();
        _discardPile.Clear();
    }

    /// <summary>
    /// Begins a new turn: refill mana and draw one card.
    /// </summary>
    public void StartTurn()
    {
        CurrentMana = MaxMana;
        DrawCards(1);
        EmitSignals();
    }

    /// <summary>
    /// Draws a specified number of cards from the draw pile, reshuffling the discard pile if needed.
    /// </summary>
    public void DrawCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_hand.Count >= MaxHandSize) return;
            if (_drawPile.Count == 0)
            {
                if (_discardPile.Count == 0) return;
                ReshuffleDiscard();
            }
            _hand.Add(_drawPile[0]);
            _drawPile.RemoveAt(0);
        }
        EmitSignals();
    }

    /// <summary>
    /// Moves all discard pile cards back into the draw pile and shuffles them.
    /// </summary>
    private void ReshuffleDiscard()
    {
        _drawPile = new List<Card>(_discardPile);
        var rng = new Random();
        for (int i = _drawPile.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            var temp = _drawPile[i];
            _drawPile[i] = _drawPile[j];
            _drawPile[j] = temp;
        }
        _discardPile.Clear();
    }

    /// <summary>
    /// Plays a card from the hand at the given index. Validates mana cost and applies effects.
    /// </summary>
    /// <returns>True if the card was successfully played.</returns>
    public bool PlayCard(int cardIndex, Node target = null)
    {
        if (cardIndex >= _hand.Count) return false;
        var card = _hand[cardIndex];
        if (CurrentMana < card.Cost) return false;

        CurrentMana -= card.Cost;
        _hand.RemoveAt(cardIndex);
        ApplyCardEffects(card, target);
        _discardPile.Add(card);
        EmitSignals();
        return true;
    }

    /// <summary>
    /// Applies damage, healing, shield, and draw effects from the played card.
    /// </summary>
    private void ApplyCardEffects(Card card, Node target)
    {
        if (card.Damage > 0 && target != null && target.HasMethod("TakeDamage"))
            target.Call("TakeDamage", card.Damage);

        if (card.Heal > 0)
            CurrentHealth = Mathf.Min(CurrentHealth + card.Heal, MaxHealth);

        if (card.Shield > 0)
            Shield += card.Shield;

        if (card.DrawCards > 0)
            DrawCards(card.DrawCards);
    }

    /// <summary>
    /// Reduces health by the given amount, factoring in shield absorption. Triggers combat end if health reaches zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (Shield > 0)
        {
            int blocked = Mathf.Min(Shield, amount);
            Shield -= blocked;
            amount -= blocked;
        }

        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            CombatEnd?.Invoke(false);
        }
        EmitSignals();
    }

    /// <summary>
    /// Ends the current turn: discard remaining hand cards.
    /// </summary>
    public void EndTurn()
    {
        foreach (var card in _hand)
            _discardPile.Add(card);
        _hand.Clear();
        EmitSignals();
    }

    /// <summary>
    /// Fires all state-change events so the UI can update.
    /// </summary>
    private void EmitSignals()
    {
        HandUpdated?.Invoke(new List<Card>(_hand));
        ManaUpdated?.Invoke(CurrentMana, MaxMana);
        HealthUpdated?.Invoke(CurrentHealth, MaxHealth);
    }
}
