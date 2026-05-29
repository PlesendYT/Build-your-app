using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Manages AR session lifecycle, plane detection, and image tracking.
/// </summary>
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
        // Auto-assign AR components if not set in inspector
        if (raycastManager == null)
            raycastManager = GetComponent<ARRaycastManager>();
        if (planeManager == null)
            planeManager = GetComponent<ARPlaneManager>();
        if (imageManager == null)
            imageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        // Subscribe to image tracking events
        if (imageManager != null && enableImageTracking)
            imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe from image tracking events
        if (imageManager != null)
            imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    /// <summary>
    /// Called when tracked images are added, updated, or removed.
    /// </summary>
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // Handle newly detected images
        foreach (ARTrackedImage image in args.added)
        {
            HandleImageTracked(image);
        }

        // Handle updated images that are actively tracked
        foreach (ARTrackedImage image in args.updated)
        {
            if (image.trackingState == TrackingState.Tracking)
                HandleImageTracked(image);
        }
    }

    /// <summary>
    /// Handles a tracked image by instantiating content at its position.
    /// </summary>
    private void HandleImageTracked(ARTrackedImage image)
    {
        if (image.trackingState == TrackingState.Tracking)
        {
            InstantiateContent(image.referenceImage.name, image.transform.position, image.transform.rotation);
        }
    }

    /// <summary>
    /// Instantiates content for a detected image. Override in subclass.
    /// </summary>
    public virtual void InstantiateContent(string imageName, Vector3 position, Quaternion rotation)
    {
        // Override in subclass to instantiate specific content
    }

    /// <summary>
    /// Gets the primary touch position if available.
    /// </summary>
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

    /// <summary>
    /// Enables or disables plane detection and toggles visibility of existing planes.
    /// </summary>
    public void TogglePlaneDetection(bool enabled)
    {
        if (planeManager != null)
        {
            planeManager.enabled = enabled;
            // Show or hide all tracked planes
            foreach (ARPlane plane in planeManager.trackables)
                plane.gameObject.SetActive(enabled);
        }
    }

    /// <summary>
    /// Resets the AR session to clear all tracked data.
    /// </summary>
    public void ResetARSession()
    {
        if (arSession != null)
            arSession.Reset();
    }
}
