using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball")]
    public float speed = 8f;
    public float maxBounceAngle = 75f;
    private Rigidbody2D rb;
    private bool isLaunched;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch()
    {
        isLaunched = true;
        float angle = Random.Range(-30f, 30f);
        Vector2 dir = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), 1f).normalized;
        rb.linearVelocity = dir * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            float hitPos = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
            float angle = hitPos * maxBounceAngle;
            Vector2 dir = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), 1f).normalized;
            rb.linearVelocity = dir * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Drain"))
        {
            BreakoutManager.Instance.BallLost();
            Destroy(gameObject);
        }
    }
}
