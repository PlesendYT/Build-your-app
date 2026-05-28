using UnityEngine;
using UnityEngine.InputSystem;

public class PlungerController : MonoBehaviour
{
    [Header("Plunger")]
    public float maxForce = 20f;
    public float chargeRate = 10f;
    private float currentForce;
    private bool charging;
    private SpringJoint2D springJoint;

    private void Awake()
    {
        springJoint = GetComponent<SpringJoint2D>();
    }

    public void OnPlunger(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            charging = true;
            currentForce = 0f;
        }
        else if (context.canceled && charging)
        {
            charging = false;
            LaunchBall();
        }
    }

    private void Update()
    {
        if (charging)
        {
            currentForce = Mathf.Min(currentForce + chargeRate * Time.deltaTime, maxForce);
        }
    }

    private void LaunchBall()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(Vector2.up * currentForce, ForceMode2D.Impulse);
        }
    }
}
