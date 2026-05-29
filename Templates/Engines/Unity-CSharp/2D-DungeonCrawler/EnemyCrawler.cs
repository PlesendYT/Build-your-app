/// <summary>
/// Controls enemy turn-based behavior: takes damage, moves toward player, and attacks when in range.
/// </summary>
using UnityEngine;

public class EnemyCrawler : MonoBehaviour
{
    [Header("Combat")]
    public int health = 10;
    public int attack = 4;
    public int defense = 1;
    public int moveRange = 3;
    private PlayerCrawler player;

    /// <summary>
    /// Finds the player instance on awake.
    /// </summary>
    private void Awake()
    {
        player = FindObjectOfType<PlayerCrawler>();
    }

    /// <summary>
    /// Reduces health by damage amount and deactivates the enemy if health reaches zero.
    /// </summary>
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.AddScore(100);
        }
    }

    /// <summary>
    /// Enemy turn: attacks if adjacent to player, moves toward player if within move range.
    /// </summary>
    public void EnemyTurn()
    {
        if (player == null || !gameObject.activeInHierarchy) return;

        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= 1.5f)
        {
            player.TakeDamage(attack);
        }
        else if (dist <= moveRange)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            Vector3 movePos = transform.position + new Vector3(Mathf.Round(dir.x), Mathf.Round(dir.y), 0f);
            transform.position = movePos;
        }
    }
}
