using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

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

    private void HandleMovement()
    {
        if (!useContinuousMovement) return;

        Vector2 input = rightController.moveAction.action.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y);
        direction = xrOrigin.Camera.transform.TransformDirection(direction);
        direction.y = 0;
        direction.Normalize();

        characterController.Move(direction * continuousMoveSpeed * Time.deltaTime);
    }

    private void HandleTurning()
    {
        Vector2 input = leftController.moveAction.action.ReadValue<Vector2>();
        if (Mathf.Abs(input.x) > 0.5f)
        {
            xrOrigin.transform.Rotate(Vector3.up, input.x * turnSpeed * Time.deltaTime);
        }
    }

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

    public void ToggleMovementMode()
    {
        useContinuousMovement = !useContinuousMovement;
    }
}
