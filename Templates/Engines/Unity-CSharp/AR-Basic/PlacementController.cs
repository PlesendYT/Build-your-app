using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementController : MonoBehaviour
{
    [Header("Placement")]
    public GameObject[] objectPrefabs;
    public GameObject placementIndicator;
    public float rotationSpeed = 45f;

    private ARRaycastManager raycastManager;
    private ARManager arManager;
    private Vector2 touchPosition;
    private Pose placementPose;
    private bool isValidPlacement;
    private GameObject selectedObject;
    private int selectedPrefabIndex;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        arManager = GetComponent<ARManager>();
    }

    private void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (isValidPlacement && arManager.TryGetTouchPosition(out touchPosition))
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObject();
            }
        }

        if (selectedObject != null && Input.touchCount == 2)
        {
            Touch touch = Input.GetTouch(1);
            if (touch.phase == TouchPhase.Moved)
            {
                float rotation = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
                selectedObject.transform.Rotate(Vector3.up, rotation);
            }
        }
    }

    private void UpdatePlacementPose()
    {
        if (!arManager.TryGetTouchPosition(out touchPosition)) return;

        var hits = new System.Collections.Generic.List<ARRaycastHit>();
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            placementPose = hits[0].pose;
            isValidPlacement = true;
        }
        else
        {
            isValidPlacement = false;
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementIndicator != null)
        {
            placementIndicator.SetActive(isValidPlacement);
            if (isValidPlacement)
            {
                placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            }
        }
    }

    private void PlaceObject()
    {
        if (objectPrefabs == null || objectPrefabs.Length == 0) return;

        GameObject prefab = objectPrefabs[selectedPrefabIndex];
        selectedObject = Instantiate(prefab, placementPose.position, placementPose.rotation);
    }

    public void SelectPrefab(int index)
    {
        if (index >= 0 && index < objectPrefabs.Length)
            selectedPrefabIndex = index;
    }

    public void RemoveSelectedObject()
    {
        if (selectedObject != null)
            Destroy(selectedObject);
    }

    public void ClearAllObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("ARContent");
        foreach (GameObject obj in objects)
            Destroy(obj);
    }
}
