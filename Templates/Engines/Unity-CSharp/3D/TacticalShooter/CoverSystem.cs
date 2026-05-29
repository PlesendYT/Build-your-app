/// <summary>
/// Cover mechanic for tactical shooter: peeking, blind fire, and cover transitions.
/// </summary>
using UnityEngine;

public class CoverSystem : MonoBehaviour
{
    [Header("Cover State")]
    public bool isInCover, isPeekingLeft, isPeekingRight;
    public CoverDirection currentCoverDirection = CoverDirection.None;

    [Header("Settings")]
    public float coverCheckDistance = 1f, peekDistance = 0.5f;
    public LayerMask coverLayer;

    [Header("References")]
    public Transform cameraTransform, coverCheckOrigin;

    private Vector3 coverNormal, coverPosition;

    public enum CoverDirection { None, Left, Right, Up }

    private void Update()
    {
        if (!isInCover) CheckForCover();
        else CheckCoverExit();
    }

    private void CheckForCover()
    {
        Vector3[] dirs = { transform.forward, -transform.forward, transform.right, -transform.right };
        foreach (Vector3 dir in dirs)
            if (Physics.Raycast(coverCheckOrigin.position, dir, out RaycastHit hit, coverCheckDistance, coverLayer))
            { EnterCover(hit); return; }
    }

    private void EnterCover(RaycastHit hit)
    {
        isInCover = true;
        coverNormal = hit.normal;
        coverPosition = hit.point - hit.normal * 0.3f;
        float dot = Vector3.Dot(transform.right, hit.normal);
        currentCoverDirection = Mathf.Abs(dot) > 0.5f ? (dot > 0 ? CoverDirection.Right : CoverDirection.Left) : CoverDirection.Up;
        transform.position = coverPosition;
    }

    private void CheckCoverExit()
    {
        if ((transform.position - coverPosition).magnitude > coverCheckDistance * 1.5f) ExitCover();
        else if (!Physics.Raycast(coverPosition, coverNormal, 0.5f, coverLayer)) ExitCover();
    }

    private void ExitCover() { isInCover = false; isPeekingLeft = false; isPeekingRight = false; currentCoverDirection = CoverDirection.None; }

    public void PeekLeft() { if (!isInCover) return; isPeekingLeft = true; isPeekingRight = false; SetPeek(-peekDistance); }
    public void PeekRight() { if (!isInCover) return; isPeekingRight = true; isPeekingLeft = false; SetPeek(peekDistance); }
    public void ResetPeek() { isPeekingLeft = false; isPeekingRight = false; SetPeek(0f); }

    private void SetPeek(float offset)
    {
        Vector3 p = cameraTransform.localPosition;
        p.x = offset;
        cameraTransform.localPosition = p;
    }

    public bool CanShootFromCover() => isPeekingLeft || isPeekingRight;
}
