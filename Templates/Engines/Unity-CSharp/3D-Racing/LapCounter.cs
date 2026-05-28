using UnityEngine;

public class LapCounter : MonoBehaviour
{
    [Header("Settings")]
    public int totalLaps = 3;
    public int currentLap = 1;
    public int currentWaypoint;
    public Transform[] waypoints;

    [Header("UI")]
    public TMPro.TextMeshProUGUI lapText;
    public TMPro.TextMeshProUGUI positionText;

    private void Start()
    {
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waypoint"))
        {
            int waypointIndex = System.Array.IndexOf(waypoints, other.transform);
            if (waypointIndex == (currentWaypoint + 1) % waypoints.Length)
            {
                currentWaypoint = waypointIndex;
                if (currentWaypoint == 0)
                {
                    currentLap++;
                    if (currentLap > totalLaps)
                    {
                        GameManager.Instance.RaceFinished();
                    }
                }
                UpdateUI();
            }
        }
    }

    private void UpdateUI()
    {
        if (lapText != null)
            lapText.text = "Lap " + Mathf.Min(currentLap, totalLaps) + "/" + totalLaps;
    }
}
