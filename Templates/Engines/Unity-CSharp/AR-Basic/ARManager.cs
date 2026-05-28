using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour
{
    [Header("AR Components")]
    public ARSession arSession;
    public ARSessionOrigin sessionOrigin;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public ARTrackedImageManager imageManager;

    [Header("Settings")]
    public bool enablePlaneDetection = true;
    public bool enableImageTracking = true;

    private void Awake()
    {
        if (raycastManager == null)
            raycastManager = GetComponent<ARRaycastManager>();
        if (planeManager == null)
            planeManager = GetComponent<ARPlaneManager>();
        if (imageManager == null)
            imageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        if (imageManager != null && enableImageTracking)
            imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        if (imageManager != null)
            imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage image in args.added)
        {
            HandleImageTracked(image);
        }

        foreach (ARTrackedImage image in args.updated)
        {
            if (image.trackingState == TrackingState.Tracking)
                HandleImageTracked(image);
        }
    }

    private void HandleImageTracked(ARTrackedImage image)
    {
        if (image.trackingState == TrackingState.Tracking)
        {
            InstantiateContent(image.referenceImage.name, image.transform.position, image.transform.rotation);
        }
    }

    public virtual void InstantiateContent(string imageName, Vector3 position, Quaternion rotation)
    {
        // Override in subclass to instantiate specific content
    }

    public bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    public void TogglePlaneDetection(bool enabled)
    {
        if (planeManager != null)
        {
            planeManager.enabled = enabled;
            foreach (ARPlane plane in planeManager.trackables)
                plane.gameObject.SetActive(enabled);
        }
    }

    public void ResetARSession()
    {
        if (arSession != null)
            arSession.Reset();
    }
}
