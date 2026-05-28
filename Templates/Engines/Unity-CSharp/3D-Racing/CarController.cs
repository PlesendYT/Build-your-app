using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float maxMotorTorque = 1500f;
    public float maxSteeringAngle = 30f;
    public float brakeTorque = 3000f;
    public float maxSpeed = 200f;

    [Header("Wheels")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;
    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    private float motorInput;
    private float steeringInput;
    private bool isBraking;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        HandleBrakes();
        UpdateWheelVisuals();
    }

    private void HandleMotor()
    {
        float speed = rb.linearVelocity.magnitude * 3.6f;
        if (speed < maxSpeed)
        {
            frontLeft.motorTorque = motorInput * maxMotorTorque;
            frontRight.motorTorque = motorInput * maxMotorTorque;
        }
        else
        {
            frontLeft.motorTorque = 0;
            frontRight.motorTorque = 0;
        }
    }

    private void HandleSteering()
    {
        frontLeft.steerAngle = steeringInput * maxSteeringAngle;
        frontRight.steerAngle = steeringInput * maxSteeringAngle;
    }

    private void HandleBrakes()
    {
        float brake = isBraking ? brakeTorque : 0f;
        rearLeft.brakeTorque = brake;
        rearRight.brakeTorque = brake;
    }

    private void UpdateWheelVisuals()
    {
        ApplyWheelVisual(frontLeft, frontLeftTransform);
        ApplyWheelVisual(frontRight, frontRightTransform);
        ApplyWheelVisual(rearLeft, rearLeftTransform);
        ApplyWheelVisual(rearRight, rearRightTransform);
    }

    private void ApplyWheelVisual(WheelCollider collider, Transform visual)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        visual.position = pos;
        visual.rotation = rot;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        motorInput = input.y;
        steeringInput = input.x;
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        isBraking = context.performed;
    }

    public float GetSpeed() => rb.linearVelocity.magnitude * 3.6f;
}
