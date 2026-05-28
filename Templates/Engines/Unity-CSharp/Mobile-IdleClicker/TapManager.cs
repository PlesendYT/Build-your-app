using UnityEngine;
using UnityEngine.EventSystems;

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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                HandleTap(touch.position);
            }
        }
    }

    private void HandleTap(Vector2 screenPos)
    {
        double earnings = tapPower * tapMultiplier;
        GameManager.Instance.AddResource(earnings);

        if (tapEffect != null)
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;
            Instantiate(tapEffect, worldPos, Quaternion.identity);
        }
    }

    public void UpgradeTapPower(float amount)
    {
        tapPower += amount;
    }

    public void MultiplyTapPower(float multiplier)
    {
        tapMultiplier *= multiplier;
    }
}
