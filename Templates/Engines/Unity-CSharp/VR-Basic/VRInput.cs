using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Handles VR movement (continuous and teleport) and turning via controller input.
/// </summary>
public class VRInput : MonoBehaviour
{
    [Header("References")]
    public XRRayInteractor leftRayInteractor;
    public XRRayInteractor rightRayInteractor;
    public TeleportationProvider teleportProvider;
    public ActionBasedController leftController;
    public ActionBasedController rightController;

    [Header("Settings")]
    public float continuousMoveSpeed = 3f;
    public float turnSpeed = 60f;

    private CharacterController characterController;
    private XROrigin xrOrigin;
    private bool useContinuousMovement;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        xrOrigin = GetComponent<XROrigin>();
    }

    private void Update()
    {
        HandleMovement();
        HandleTurning();
    }

    /// <summary>
    /// Moves the player continuously if continuous movement mode is active.
    /// </summary>
    private void HandleMovement()
    {
        if (!useContinuousMovement) return;

        // Read movement input from the right controller joystick
        Vector2 input = rightController.moveAction.action.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y);
        direction = xrOrigin.Camera.transform.TransformDirection(direction);
        direction.y = 0;
        direction.Normalize();

        characterController.Move(direction * continuousMoveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Rotates the player using the left controller joystick horizontal input.
    /// </summary>
    private void HandleTurning()
    {
        Vector2 input = leftController.moveAction.action.ReadValue<Vector2>();
        if (Mathf.Abs(input.x) > 0.5f)
        {
            xrOrigin.transform.Rotate(Vector3.up, input.x * turnSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Queues a teleport request to the specified position and rotation.
    /// </summary>
    public void Teleport(Vector3 position, Quaternion rotation)
    {
        if (teleportProvider != null)
        {
            var request = new TeleportRequest
            {
                destinationPosition = position,
                destinationRotation = rotation,
                matchOrientation = MatchOrientation.TargetUpAndForward
            };
            teleportProvider.QueueTeleportRequest(request);
        }
    }

    /// <summary>
    /// Toggles between teleport and continuous movement modes.
    /// </summary>
    public void ToggleMovementMode()
    {
        useContinuousMovement = !useContinuousMovement;
    }
}
