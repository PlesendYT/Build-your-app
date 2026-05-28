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

    private void Awake()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
    }

    public void OnFlipper(InputAction.CallbackContext context)
    {
        bool pressed = context.performed;
        JointMotor2D motor = hingeJoint.motor;
        motor.motorSpeed = pressed ? (isLeftFlipper ? -force : force) : (isLeftFlipper ? force : -force);
        hingeJoint.motor = motor;
    }
}
