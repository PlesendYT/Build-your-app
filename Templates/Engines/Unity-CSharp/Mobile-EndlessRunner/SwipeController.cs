using UnityEngine;

/// <summary>
/// Detects swipe gestures on touch input and identifies swipe direction.
/// </summary>
public class SwipeController : MonoBehaviour
{
    [Header("Swipe Settings")]
    public float minSwipeDistance = 50f;
    public float maxSwipeTime = 0.5f;

    private Vector2 touchStartPos;
    private float touchStartTime;
    private bool isSwiping;

    /// <summary>
    /// Enum representing possible swipe directions.
    /// </summary>
    public enum SwipeDirection { None, Left, Right, Up, Down }

    /// <summary>
    /// The most recently detected swipe direction.
    /// </summary>
    public SwipeDirection lastSwipe { get; private set; }

    private void Update()
    {
        // Reset swipe each frame before processing input
        lastSwipe = SwipeDirection.None;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Record start position and time when touch begins
                    touchStartPos = touch.position;
                    touchStartTime = Time.time;
                    isSwiping = true;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    // Detect swipe direction when touch ends
                    if (isSwiping)
                    {
                        lastSwipe = DetectSwipe(touch.position);
                        isSwiping = false;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Determines the swipe direction based on start and end positions.
    /// </summary>
    private SwipeDirection DetectSwipe(Vector2 touchEndPos)
    {
        // Reject if swipe took too long
        float swipeTime = Time.time - touchStartTime;
        if (swipeTime > maxSwipeTime) return SwipeDirection.None;

        Vector2 delta = touchEndPos - touchStartPos;
        float distance = delta.magnitude;

        // Reject if swipe distance is too short
        if (distance < minSwipeDistance) return SwipeDirection.None;

        // Determine direction based on angle of the swipe vector
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        if (angle > -45f && angle <= 45f) return SwipeDirection.Right;
        if (angle > 45f && angle <= 135f) return SwipeDirection.Up;
        if (angle > -135f && angle <= -45f) return SwipeDirection.Down;
        return SwipeDirection.Left;
    }

    /// <summary>
    /// Returns true if the screen is currently being touched.
    /// </summary>
    public bool IsTouching()
    {
        return Input.touchCount > 0;
    }
}
