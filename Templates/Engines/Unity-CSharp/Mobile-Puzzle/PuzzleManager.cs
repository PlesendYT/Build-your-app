using UnityEngine;

/// <summary>
/// Manages puzzle generation, piece placement tracking, level progression, and UI updates.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public int gridWidth = 3;
    public int gridHeight = 3;
    public GameObject piecePrefab;
    public Transform puzzleContainer;

    [Header("Level")]
    public int currentLevel;
    public int piecesPlaced;
    public int totalPieces;

    [Header("UI")]
    public TMPro.TextMeshProUGUI levelText;
    public TMPro.TextMeshProUGUI progressText;
    public GameObject levelCompletePanel;

    private PuzzlePiece[] pieces;

    private void Start()
    {
        totalPieces = gridWidth * gridHeight;
        GeneratePuzzle();
        UpdateUI();
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }

    /// <summary>
    /// Generates a new puzzle by instantiating pieces at random positions.
    /// </summary>
    public void GeneratePuzzle()
    {
        if (piecePrefab == null) return;

        pieces = new PuzzlePiece[totalPieces];
        int index = 0;

        // Create pieces for each grid cell with shuffled positions
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2Int correct = new Vector2Int(x, y);
                Vector2Int randomPos = GetRandomAvailablePosition(index);

                GameObject obj = Instantiate(piecePrefab, puzzleContainer);
                PuzzlePiece piece = obj.GetComponent<PuzzlePiece>();
                piece.pieceId = index;
                piece.correctPosition = correct;
                piece.gridPosition = randomPos;
                obj.transform.position = new Vector3(randomPos.x, randomPos.y, 0);

                pieces[index] = piece;
                index++;
            }
        }
    }

    /// <summary>
    /// Returns a random grid position for piece placement during generation.
    /// </summary>
    private Vector2Int GetRandomAvailablePosition(int placed)
    {
        // Simple shuffle - emit random position
        return new Vector2Int(
            Random.Range(0, gridWidth * 2),
            Random.Range(0, gridHeight * 2)
        );
    }

    /// <summary>
    /// Called when a piece is correctly placed. Checks for level completion.
    /// </summary>
    public void RegisterPiecePlaced()
    {
        piecesPlaced++;
        UpdateUI();

        // Trigger level complete when all pieces are placed
        if (piecesPlaced >= totalPieces)
        {
            LevelComplete();
        }
    }

    /// <summary>
    /// Shows the level complete panel.
    /// </summary>
    private void LevelComplete()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);
    }

    /// <summary>
    /// Advances to the next level by clearing the current puzzle and generating a new one.
    /// </summary>
    public void NextLevel()
    {
        currentLevel++;
        piecesPlaced = 0;

        // Destroy all existing piece objects
        foreach (Transform child in puzzleContainer)
            Destroy(child.gameObject);

        GeneratePuzzle();
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
        UpdateUI();
    }

    /// <summary>
    /// Refreshes the level and progress UI text.
    /// </summary>
    private void UpdateUI()
    {
        if (levelText != null) levelText.text = "Level " + currentLevel;
        if (progressText != null) progressText.text = piecesPlaced + "/" + totalPieces;
    }
}
