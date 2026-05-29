/// <summary>
/// Manages puzzle game state: tracks move count, target blocks, and win condition.
/// </summary>
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

    /// <summary>
    /// Initializes the move counter and counts total target tiles.
    /// </summary>
    private void Start()
    {
        moveCount = 0;
        totalTargets = CountTargets();
        completedTargets = 0;
        UpdateUI();
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }

    /// <summary>
    /// Counts the number of target tiles on the grid.
    /// </summary>
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

    /// <summary>
    /// Increments the move counter and updates the UI.
    /// </summary>
    public void RegisterMove()
    {
        moveCount++;
        UpdateUI();
    }

    /// <summary>
    /// Registers a block reaching a target and checks the win condition.
    /// </summary>
    public void RegisterBlockOnTarget()
    {
        completedTargets++;
        CheckWinCondition();
    }

    /// <summary>
    /// Registers a block leaving a target.
    /// </summary>
    public void RegisterBlockOffTarget()
    {
        completedTargets--;
    }

    /// <summary>
    /// Checks if all targets are covered by blocks and shows the level complete panel.
    /// </summary>
    private void CheckWinCondition()
    {
        if (completedTargets >= totalTargets && totalTargets > 0)
        {
            if (levelCompletePanel != null)
                levelCompletePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    /// <summary>
    /// Updates the move counter text display.
    /// </summary>
    private void UpdateUI()
    {
        if (moveCounterText != null)
            moveCounterText.text = "Moves: " + moveCount;
    }

    /// <summary>
    /// Loads the next level scene.
    /// </summary>
    public void NextLevel()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
