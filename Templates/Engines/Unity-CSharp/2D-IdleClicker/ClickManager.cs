using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    [Header("Click Settings")]
    public float baseClickPower = 1f;
    public float clickMultiplier = 1f;

    public float TotalClickPower => baseClickPower * clickMultiplier;

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed && !EventSystem.current.IsPointerOverGameObject())
        {
            GameManager.Instance.AddResource(TotalClickPower);
        }
    }

    public void UpgradeClickPower(float amount)
    {
        baseClickPower += amount;
    }

    public void MultiplyClickPower(float multiplier)
    {
        clickMultiplier *= multiplier;
    }
}
