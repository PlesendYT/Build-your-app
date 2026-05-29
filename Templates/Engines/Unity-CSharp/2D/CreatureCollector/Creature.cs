/// <summary>
/// Defines a collectible creature with stats, moves, leveling, and evolution data.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCreature", menuName = "Creature Collector/Creature")]
public class Creature : ScriptableObject
{
    [Header("Basic Info")]
    public string creatureName = "Flamepaw";
    public int id;
    public Sprite frontSprite;
    public Sprite backSprite;
    public ElementType element;

    [Header("Stats")]
    public int level = 5;
    public int maxHealth = 50;
    public int currentHealth;
    public int attack = 10;
    public int defense = 8;
    public int speed = 12;

    [Header("Moves")]
    public List<Move> learnedMoves = new List<Move>();
    public int maxMoves = 4;

    [Header("Evolution")]
    public Creature evolvesInto;
    public int evolutionLevel = 16;

    public enum ElementType { Normal, Fire, Water, Grass, Electric, Ice }

    [System.Serializable]
    public class Move
    {
        public string moveName;
        public int power;
        public ElementType moveElement;
        public int pp;
    }

    /// <summary>
    /// Initializes health to max on creation.
    /// </summary>
    private void OnEnable()
    {
        if (currentHealth == 0)
            currentHealth = maxHealth;
    }

    /// <summary>
    /// Calculates damage dealt to another creature using the attack and defense stats.
    /// </summary>
    public int CalculateDamage(Creature target, Move move)
    {
        float modifier = 1f;
        if (move.moveElement == Element.Fire && target.element == Element.Grass)
            modifier = 2f;
        else if (move.moveElement == Element.Water && target.element == Element.Fire)
            modifier = 2f;

        int damage = Mathf.FloorToInt((attack * move.power / (float)target.defense) * modifier);
        return Mathf.Max(1, damage);
    }

    /// <summary>
    /// Reduces health by the damage amount.
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
    }

    /// <summary>
    /// Checks if the creature should evolve and returns the evolved form if applicable.
    /// </summary>
    public Creature CheckEvolution()
    {
        if (evolvesInto != null && level >= evolutionLevel)
        {
            Debug.Log(creatureName + " evolved into " + evolvesInto.creatureName);
            return evolvesInto;
        }
        return this;
    }
}
