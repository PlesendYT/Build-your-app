using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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
        if (grabbedObject != null)
        {
            Vector3 currentPosition = grabbedObject.transform.position;
            velocity = (currentPosition - previousPosition) / Time.deltaTime;
            previousPosition = currentPosition;
        }
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject is XRGrabInteractable grabInteractable)
        {
            grabbedObject = grabInteractable;
            if (grabbedObject != null)
            {
                grabbedObject.trackPosition = true;
                grabbedObject.trackRotation = true;
                grabbedObject.throwOnDetach = true;
                grabbedObject.throwVelocityScale = throwForce;
            }
        }
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        if (grabbedObject != null)
        {
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
