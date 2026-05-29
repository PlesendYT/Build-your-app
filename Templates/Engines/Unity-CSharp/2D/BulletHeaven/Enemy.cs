/// <summary>
/// Represents an enemy in the bullet heaven game: moves toward the player, deals contact damage, drops XP.
/// </summary>
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int health = 20;
    public int damage = 5;
    public float moveSpeed = 2f;
    public int xpReward = 10;

    [Header("References")]
    private Transform player;

    /// <summary>
    /// Finds the player as the movement target.
    /// </summary>
    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    /// <summary>
    /// Moves toward the player each frame.
    /// </summary>
    private void Update()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Reduces health by the damage amount and handles death.
    /// </summary>
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Notifies the game manager, drops XP gem, and destroys this enemy.
    /// </summary>
    private void Die()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
            gm.OnEnemyKilled(xpReward);

        // Drop an XP gem for the player to collect
        GameObject gem = new GameObject("XPGem");
        gem.tag = "XP";
        gem.transform.position = transform.position;
        XPGem xpGem = gem.AddComponent<XPGem>();
        xpGem.xpAmount = xpReward;

        Destroy(gameObject);
    }

    /// <summary>
    /// Deals damage to the player on contact.
    /// </summary>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
                gm.AddXP(-damage); // placeholder — in real logic, reduce player health
        }
    }
}
