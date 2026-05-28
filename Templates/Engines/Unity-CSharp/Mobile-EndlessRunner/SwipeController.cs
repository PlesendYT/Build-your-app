using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [Header("Swipe Settings")]
    public float minSwipeDistance = 50f;
    public float maxSwipeTime = 0.5f;

    private Vector2 touchStartPos;
    private float touchStartTime;
    private bool isSwiping;

    public enum SwipeDirection { None, Left, Right, Up, Down }
    public SwipeDirection lastSwipe { get; private set; }

    private void Update()
    {
        lastSwipe = SwipeDirection.None;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    touchStartTime = Time.time;
                    isSwiping = true;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isSwiping)
                    {
                        lastSwipe = DetectSwipe(touch.position);
                        isSwiping = false;
                    }
                    break;
            }
        }
    }

    private SwipeDirection DetectSwipe(Vector2 touchEndPos)
    {
        float swipeTime = Time.time - touchStartTime;
        if (swipeTime > maxSwipeTime) return SwipeDirection.None;

        Vector2 delta = touchEndPos - touchStartPos;
        float distance = delta.magnitude;

        if (distance < minSwipeDistance) return SwipeDirection.None;

        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        if (angle > -45f && angle <= 45f) return SwipeDirection.Right;
        if (angle > 45f && angle <= 135f) return SwipeDirection.Up;
        if (angle > -135f && angle <= -45f) return SwipeDirection.Down;
        return SwipeDirection.Left;
    }

    public bool IsTouching()
    {
        return Input.touchCount > 0;
    }
}
