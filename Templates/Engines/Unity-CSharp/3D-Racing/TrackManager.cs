using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public Transform startPosition;
    public Transform[] aiWaypoints;

    public Vector3 GetStartPosition()
    {
        return startPosition != null ? startPosition.position : Vector3.zero;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject raceCompletePanel;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void RaceFinished()
    {
        if (raceCompletePanel != null)
            raceCompletePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
