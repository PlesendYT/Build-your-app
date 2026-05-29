/// <summary>
/// Handles click input for the idle clicker game, generating resources per click with upgradeable power and multipliers.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    [Header("Click Settings")]
    public float baseClickPower = 1f;
    public float clickMultiplier = 1f;

    /// <summary>
    /// Total resource generation per click, combining base power and multiplier.
    /// </summary>
    public float TotalClickPower => baseClickPower * clickMultiplier;

    /// <summary>
    /// Adds resources on click if not hovering over a UI element.
    /// </summary>
    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed && !EventSystem.current.IsPointerOverGameObject())
        {
            GameManager.Instance.AddResource(TotalClickPower);
        }
    }

    /// <summary>
    /// Increases the base click power by a fixed amount.
    /// </summary>
    public void UpgradeClickPower(float amount)
    {
        baseClickPower += amount;
    }

    /// <summary>
    /// Multiplies the click power by a given factor.
    /// </summary>
    public void MultiplyClickPower(float multiplier)
    {
        clickMultiplier *= multiplier;
    }
}
