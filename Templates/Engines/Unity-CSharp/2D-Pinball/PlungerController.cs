/// <summary>
/// Controls the pinball plunger that charges force while held and launches the ball on release.
/// </summary>
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

    /// <summary>
    /// Caches the spring joint component.
    /// </summary>
    private void Awake()
    {
        springJoint = GetComponent<SpringJoint2D>();
    }

    /// <summary>
    /// Starts charging on input start, launches on input release.
    /// </summary>
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

    /// <summary>
    /// Increases the charge force over time while charging, up to the maximum.
    /// </summary>
    private void Update()
    {
        if (charging)
        {
            currentForce = Mathf.Min(currentForce + chargeRate * Time.deltaTime, maxForce);
        }
    }

    /// <summary>
    /// Applies the accumulated force to launch the ball upward.
    /// </summary>
    private void LaunchBall()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(Vector2.up * currentForce, ForceMode2D.Impulse);
        }
    }
}
