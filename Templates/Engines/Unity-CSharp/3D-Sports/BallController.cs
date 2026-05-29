/// <summary>
/// Ball with physics, friction, max speed, and goal detection for scoring.
/// </summary>
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Physics")]
    public float maxSpeed = 30f;
    public float bounceFactor = 0.8f;
    public float friction = 0.98f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 10f;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity *= friction;
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    public void Kick(Vector3 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            ScoreManager.Instance.Score(collision.gameObject.name == "GoalPlayer" ? 1 : 0);
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = Vector3.zero;
    }
}
