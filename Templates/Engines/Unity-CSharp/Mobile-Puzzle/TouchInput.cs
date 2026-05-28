using UnityEngine;

public class TouchInput : MonoBehaviour
{
    [Header("Settings")]
    public float dragThreshold = 10f;

    private Camera mainCamera;
    private PuzzlePiece draggedPiece;
    private Vector3 offset;
    private Vector3 originalPosition;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(touch.position);
            worldPos.z = 0;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    TryPickup(worldPos);
                    break;

                case TouchPhase.Moved:
                    if (draggedPiece != null)
                        draggedPiece.transform.position = worldPos + offset;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (draggedPiece != null)
                    {
                        draggedPiece.TrySnap();
                        draggedPiece = null;
                    }
                    break;
            }
        }
    }

    private void TryPickup(Vector3 worldPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        if (hit != null && hit.TryGetComponent<PuzzlePiece>(out var piece))
        {
            if (!piece.isLocked)
            {
                draggedPiece = piece;
                offset = piece.transform.position - worldPos;
                originalPosition = piece.transform.position;
                piece.transform.SetAsLastSibling();
            }
        }
    }
}
