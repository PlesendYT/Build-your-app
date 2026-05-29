using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles tap input for the idle clicker, converting screen taps into resource earnings.
/// </summary>
public class TapManager : MonoBehaviour
{
    [Header("Tap Settings")]
    public float tapPower = 1f;
    public float tapMultiplier = 1f;
    public GameObject tapEffect;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Detect touch begin events that are not over UI elements
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                HandleTap(touch.position);
            }
        }
    }

    /// <summary>
    /// Adds resources based on tap power and shows a visual tap effect.
    /// </summary>
    private void HandleTap(Vector2 screenPos)
    {
        double earnings = tapPower * tapMultiplier;
        GameManager.Instance.AddResource(earnings);

        // Spawn tap effect at the touch position in world space
        if (tapEffect != null)
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;
            Instantiate(tapEffect, worldPos, Quaternion.identity);
        }
    }

    /// <summary>
    /// Increases the base tap power by the given amount.
    /// </summary>
    public void UpgradeTapPower(float amount)
    {
        tapPower += amount;
    }

    /// <summary>
    /// Multiplies the tap power multiplier by the given factor.
    /// </summary>
    public void MultiplyTapPower(float multiplier)
    {
        tapMultiplier *= multiplier;
    }
}
