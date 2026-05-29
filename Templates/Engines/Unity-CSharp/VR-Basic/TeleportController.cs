using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Manages VR teleportation with raycasting, reticle display, and input handling.
/// </summary>
public class TeleportController : MonoBehaviour
{
    [Header("Teleport")]
    public XRRayInteractor rayInteractor;
    public GameObject teleportReticle;
    public LayerMask teleportMask;
    public float maxTeleportDistance = 10f;

    private VRInput vrInput;
    private bool isTeleporting;

    private void Awake()
    {
        vrInput = GetComponent<VRInput>();
    }

    private void Update()
    {
        if (rayInteractor == null) return;

        // Check if the ray hits a valid teleport surface
        bool hasHit = rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit);
        bool canTeleport = hasHit && ((1 << hit.collider.gameObject.layer) & teleportMask) != 0;

        // Show or hide the teleport reticle at the hit point
        if (teleportReticle != null)
        {
            teleportReticle.SetActive(canTeleport);
            if (canTeleport)
                teleportReticle.transform.position = hit.point;
        }

        // Trigger teleport if the surface is valid and input is detected
        if (canTeleport && CheckTeleportInput())
        {
            vrInput.Teleport(hit.point, Quaternion.identity);
        }
    }

    /// <summary>
    /// Checks whether the teleport activate action was pressed this frame.
    /// </summary>
    private bool CheckTeleportInput()
    {
        return rayInteractor.TryGetComponent<ActionBasedController>(out var controller) &&
               controller.activateAction.action.WasPressedThisFrame();
    }
}
