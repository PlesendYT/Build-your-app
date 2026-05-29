/// <summary>
/// Defines an extraction point trigger that ends the raid and scores loot when the player enters.
/// </summary>
using UnityEngine;

public class ExtractionPoint : MonoBehaviour
{
    [Header("Extraction Settings")]
    public string pointName = "Extraction Zone";
    public float extractionTime = 5f;
    public bool isActive = true;
    private float extractionTimer;
    private bool isExtracting;

    /// <summary>
    /// Tracks the extraction timer while the player is inside the zone.
    /// </summary>
    private void Update()
    {
        if (!isActive || !isExtracting) return;

        extractionTimer -= Time.deltaTime;
        if (extractionTimer <= 0)
        {
            CompleteExtraction();
        }
    }

    /// <summary>
    /// When the player enters the trigger, begins the extraction countdown.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            isExtracting = true;
            extractionTimer = extractionTime;
            Debug.Log("Entering " + pointName + ". Hold still for " + extractionTime + " seconds.");
        }
    }

    /// <summary>
    /// If the player leaves the zone, cancels the extraction countdown.
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            isExtracting = false;
            extractionTimer = extractionTime;
            Debug.Log("Extraction cancelled — left the zone.");
        }
    }

    /// <summary>
    /// Finalizes extraction: scores all loot and ends the raid.
    /// </summary>
    private void CompleteExtraction()
    {
        isExtracting = false;
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            int score = gm.Extract();
            Debug.Log("Successfully extracted from " + pointName + " with " + score + " points!");
        }
    }

    /// <summary>
    /// Toggles whether this extraction point can be used.
    /// </summary>
    public void SetActive(bool state)
    {
        isActive = state;
    }
}
