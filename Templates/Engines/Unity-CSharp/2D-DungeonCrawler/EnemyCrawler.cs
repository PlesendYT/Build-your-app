using UnityEngine;

public class EnemyCrawler : MonoBehaviour
{
    [Header("Combat")]
    public int health = 10;
    public int attack = 4;
    public int defense = 1;
    public int moveRange = 3;
    private PlayerCrawler player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerCrawler>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.AddScore(100);
        }
    }

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
