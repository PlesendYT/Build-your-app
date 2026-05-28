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
