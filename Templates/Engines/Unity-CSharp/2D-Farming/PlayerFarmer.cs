/// <summary>
/// Controls the farmer player: movement, interacting with field plots, and cycling through available crops.
/// </summary>
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

    /// <summary>
    /// Caches the rigidbody component.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Applies movement velocity each physics frame.
    /// </summary>
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    /// <summary>
    /// Reads movement input from the Input System.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Interacts with a nearby field plot: tills, plants, waters, or harvests based on the plot state.
    /// </summary>
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

    /// <summary>
    /// Cycles to the next available crop type.
    /// </summary>
    public void OnCycleCrop(InputAction.CallbackContext context)
    {
        if (context.performed)
            selectedCrop = (selectedCrop + 1) % availableCrops.Length;
    }
}
