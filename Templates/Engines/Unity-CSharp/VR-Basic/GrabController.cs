using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Handles grabbing, releasing, and throwing VR objects with velocity-based throw mechanics.
/// </summary>
public class GrabController : MonoBehaviour
{
    [Header("Grab Settings")]
    public float throwForce = 1.5f;
    public bool useVelocity = true;

    private XRGrabInteractable grabbedObject;
    private Vector3 previousPosition;
    private Vector3 velocity;

    private void Update()
    {
        // Track velocity of the currently grabbed object
        if (grabbedObject != null)
        {
            Vector3 currentPosition = grabbedObject.transform.position;
            velocity = (currentPosition - previousPosition) / Time.deltaTime;
            previousPosition = currentPosition;
        }
    }

    /// <summary>
    /// Called when a grab interactable is selected. Configures throw settings.
    /// </summary>
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject is XRGrabInteractable grabInteractable)
        {
            grabbedObject = grabInteractable;
            if (grabbedObject != null)
            {
                // Enable position and rotation tracking for throw calculation
                grabbedObject.trackPosition = true;
                grabbedObject.trackRotation = true;
                grabbedObject.throwOnDetach = true;
                grabbedObject.throwVelocityScale = throwForce;
            }
        }
    }

    /// <summary>
    /// Called when a grab interactable is released. Applies velocity-based throw force.
    /// </summary>
    public void OnSelectExited(SelectExitEventArgs args)
    {
        if (grabbedObject != null)
        {
            // Apply calculated velocity to the object's rigidbody on release
            if (useVelocity)
            {
                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = velocity * throwForce;
                }
            }
            grabbedObject = null;
        }
    }
}
