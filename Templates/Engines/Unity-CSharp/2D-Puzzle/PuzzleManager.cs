using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("References")]
    public GridManager gridManager;
    public TMPro.TextMeshProUGUI moveCounterText;
    public GameObject levelCompletePanel;

    private int moveCount;
    private int totalTargets;
    private int completedTargets;

    private void Start()
    {
        moveCount = 0;
        totalTargets = CountTargets();
        completedTargets = 0;
        UpdateUI();
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }

    private int CountTargets()
    {
        int count = 0;
        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            for (int y = 0; y < gridManager.gridHeight; y++)
            {
                if (gridManager.GetTileType(x, y) == GridManager.TileType.Target)
                    count++;
            }
        }
        return count;
    }

    public void RegisterMove()
    {
        moveCount++;
        UpdateUI();
    }

    public void RegisterBlockOnTarget()
    {
        completedTargets++;
        CheckWinCondition();
    }

    public void RegisterBlockOffTarget()
    {
        completedTargets--;
    }

    private void CheckWinCondition()
    {
        if (completedTargets >= totalTargets && totalTargets > 0)
        {
            if (levelCompletePanel != null)
                levelCompletePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void UpdateUI()
    {
        if (moveCounterText != null)
            moveCounterText.text = "Moves: " + moveCount;
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
