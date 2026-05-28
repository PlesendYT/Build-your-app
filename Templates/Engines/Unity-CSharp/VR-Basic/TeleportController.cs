using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

        bool hasHit = rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit);
        bool canTeleport = hasHit && ((1 << hit.collider.gameObject.layer) & teleportMask) != 0;

        if (teleportReticle != null)
        {
            teleportReticle.SetActive(canTeleport);
            if (canTeleport)
                teleportReticle.transform.position = hit.point;
        }

        if (canTeleport && CheckTeleportInput())
        {
            vrInput.Teleport(hit.point, Quaternion.identity);
        }
    }

    private bool CheckTeleportInput()
    {
        return rayInteractor.TryGetComponent<ActionBasedController>(out var controller) &&
               controller.activateAction.action.WasPressedThisFrame();
    }
}
