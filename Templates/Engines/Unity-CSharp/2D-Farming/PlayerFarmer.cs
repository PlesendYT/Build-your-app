using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFarmer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Interaction")]
    public LayerMask plotLayer;
    public float interactRange = 1.5f;
    public FieldPlot.CropData[] availableCrops;
    private int selectedCrop;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, plotLayer);
        if (hit != null)
        {
            FieldPlot plot = hit.GetComponent<FieldPlot>();
            if (plot != null)
            {
                switch (plot.state)
                {
                    case FieldPlot.PlotState.Empty:
                        plot.Till();
                        break;
                    case FieldPlot.PlotState.Tilled:
                        if (availableCrops.Length > 0)
                            plot.Plant(availableCrops[selectedCrop]);
                        break;
                    case FieldPlot.PlotState.Planted:
                    case FieldPlot.PlotState.Growing:
                        plot.Water();
                        break;
                    case FieldPlot.PlotState.Ready:
                        plot.Harvest();
                        break;
                }
            }
        }
    }

    public void OnCycleCrop(InputAction.CallbackContext context)
    {
        if (context.performed)
            selectedCrop = (selectedCrop + 1) % availableCrops.Length;
    }
}
