/// <summary>
/// Controls a pinball flipper using a hinge joint, activating on input to hit the ball.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class FlipperController : MonoBehaviour
{
    [Header("Flipper")]
    public bool isLeftFlipper;
    public float force = 10f;
    public float angleUp = -30f;
    public float angleDown = 30f;
    private HingeJoint2D hingeJoint;

    /// <summary>
    /// Caches the hinge joint component.
    /// </summary>
    private void Awake()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
    }

    /// <summary>
    /// Sets the flipper motor speed based on input (up when pressed, down when released).
    /// </summary>
    public void OnFlipper(InputAction.CallbackContext context)
    {
        bool pressed = context.performed;
        JointMotor2D motor = hingeJoint.motor;
        motor.motorSpeed = pressed ? (isLeftFlipper ? -force : force) : (isLeftFlipper ? force : -force);
        hingeJoint.motor = motor;
    }
}
