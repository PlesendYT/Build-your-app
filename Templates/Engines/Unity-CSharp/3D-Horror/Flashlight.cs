/// <summary>
/// Flashlight with battery drain/recharge, flicker, and toggle input.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [Header("Settings")]
    public Light flashlightLight;
    public float maxBattery = 100f;
    public float currentBattery;
    public float drainRate = 5f;
    public float rechargeRate = 10f;
    private bool isOn;

    [Header("Flicker")]
    public float lowBatteryThreshold = 20f;
    public float flickerSpeed = 5f;
    public float flickerIntensity = 0.5f;

    private float originalIntensity;

    private void Awake()
    {
        currentBattery = maxBattery;
        if (flashlightLight != null)
        {
            originalIntensity = flashlightLight.intensity;
            flashlightLight.enabled = false;
        }
    }

    private void Update()
    {
        if (isOn)
        {
            currentBattery -= drainRate * Time.deltaTime;
            if (currentBattery <= 0)
            {
                currentBattery = 0;
                Toggle(false);
                return;
            }

            if (currentBattery <= lowBatteryThreshold && flashlightLight != null)
            {
                float flicker = Mathf.Sin(Time.time * flickerSpeed) * flickerIntensity;
                flashlightLight.intensity = originalIntensity - Mathf.Abs(flicker);
            }
        }
        else
        {
            currentBattery = Mathf.Min(maxBattery, currentBattery + rechargeRate * Time.deltaTime);
        }
    }

    public void OnToggle(InputAction.CallbackContext context)
    {
        if (context.performed && currentBattery > 0)
        {
            Toggle(!isOn);
        }
    }

    private void Toggle(bool state)
    {
        isOn = state;
        if (flashlightLight != null)
        {
            flashlightLight.enabled = state;
            flashlightLight.intensity = originalIntensity;
        }
    }

    public float GetBattery() => currentBattery;
    public float GetMaxBattery() => maxBattery;
    public bool IsOn() => isOn;
}
