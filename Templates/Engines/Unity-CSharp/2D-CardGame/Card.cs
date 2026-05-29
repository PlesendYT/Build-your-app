/// <summary>
/// Defines a card's data as a ScriptableObject, along with CardInstance (runtime card) and PlayerController for card games.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card Game/Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public int manaCost;
    public CardType cardType;
    public int attack;
    public int health;
    public Sprite artwork;
    public EffectType effectType;
    public int effectValue;

    public enum CardType { Unit, Spell, Trap }
    public enum EffectType { None, Damage, Heal, DrawCard, Buff }
}

/// <summary>
/// Runtime instance of a card, tracking its current attack, health, and ability to be played.
/// </summary>
public class CardInstance
{
    public Card cardData;
    public int currentAttack;
    public int currentHealth;

    public CardInstance(Card data)
    {
        cardData = data;
        currentAttack = data.attack;
        currentHealth = data.health;
    }

    /// <summary>
    /// Checks if the card can be played with the available mana.
    /// </summary>
    public bool CanPlay(int currentMana)
    {
        return currentMana >= cardData.manaCost;
    }

    /// <summary>
    /// Applies the card's effect (damage, heal, or draw) to the owner or target.
    /// </summary>
    public void Play(PlayerController owner, PlayerController target)
    {
        switch (cardData.effectType)
        {
            case Card.EffectType.Damage:
                target.TakeDamage(cardData.effectValue);
                break;
            case Card.EffectType.Heal:
                owner.Heal(cardData.effectValue);
                break;
            case Card.EffectType.DrawCard:
                owner.DrawCards(cardData.effectValue);
                break;
        }
    }
}

/// <summary>
/// Represents a player in the card game with health and mana management.
/// </summary>
public class PlayerController
{
    public int health;
    public int maxHealth;
    public int mana;
    public int maxMana;

    /// <summary>
    /// Reduces health by the given amount, clamped to zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        health = Mathf.Max(0, health - amount);
    }

    /// <summary>
    /// Restores health up to the maximum value.
    /// </summary>
    public void Heal(int amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
    }

    /// <summary>
    /// Stub for drawing cards.
    /// </summary>
    public void DrawCards(int count) { }
}

public class CardInstance
{
    public Card cardData;
    public int currentAttack;
    public int currentHealth;

    public CardInstance(Card data)
    {
        cardData = data;
        currentAttack = data.attack;
        currentHealth = data.health;
    }

    public bool CanPlay(int currentMana)
    {
        return currentMana >= cardData.manaCost;
    }

    public void Play(PlayerController owner, PlayerController target)
    {
        switch (cardData.effectType)
        {
            case Card.EffectType.Damage:
                target.TakeDamage(cardData.effectValue);
                break;
            case Card.EffectType.Heal:
                owner.Heal(cardData.effectValue);
                break;
            case Card.EffectType.DrawCard:
                owner.DrawCards(cardData.effectValue);
                break;
        }
    }
}

public class PlayerController
{
    public int health;
    public int maxHealth;
    public int mana;
    public int maxMana;

    public void TakeDamage(int amount)
    {
        health = Mathf.Max(0, health - amount);
    }

    public void Heal(int amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
    }

    public void DrawCards(int count) { }
}
