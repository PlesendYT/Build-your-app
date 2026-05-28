using UnityEngine;
using System.Collections.Generic;

public class WordGrid : MonoBehaviour
{
    [Header("Grid")]
    public int gridSize = 10;
    public GameObject cellPrefab;
    public Transform gridParent;
    public float cellSize = 1f;

    [Header("Words")]
    public string[] wordList;
    public char[] fillerLetters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    private LetterCell[,] grid;
    private List<string> placedWords = new List<string>();
    private HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();

    private enum Direction { Horizontal, Vertical, DiagonalDown, DiagonalUp }

    private void Start()
    {
        grid = new LetterCell[gridSize, gridSize];
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        foreach (string word in wordList)
            TryPlaceWord(word.ToUpperInvariant());

        float startX = -(gridSize - 1) * cellSize / 2f;
        float startY = (gridSize - 1) * cellSize / 2f;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 pos = new Vector3(startX + x * cellSize, startY - y * cellSize, 0f);
                GameObject obj = Instantiate(cellPrefab, pos, Quaternion.identity, gridParent);
                LetterCell cell = obj.GetComponent<LetterCell>();
                char letter = occupied.Contains(new Vector2Int(x, y)) ? grid[x, y].letter : fillerLetters[Random.Range(0, fillerLetters.Length)];
                cell.Init(x, y, letter, this);
                grid[x, y] = cell;
            }
        }
    }

    private bool TryPlaceWord(string word)
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            int x = Random.Range(0, gridSize);
            int y = Random.Range(0, gridSize);
            Direction dir = (Direction)Random.Range(0, 4);

            if (CanPlace(word, x, y, dir))
            {
                PlaceWord(word, x, y, dir);
                placedWords.Add(word);
                return true;
            }
        }
        return false;
    }

    private bool CanPlace(string word, int x, int y, Direction dir)
    {
        int dx = 0, dy = 0;
        switch (dir)
        {
            case Direction.Horizontal: dx = 1; break;
            case Direction.Vertical: dy = 1; break;
            case Direction.DiagonalDown: dx = 1; dy = 1; break;
            case Direction.DiagonalUp: dx = 1; dy = -1; break;
        }

        for (int i = 0; i < word.Length; i++)
        {
            int nx = x + i * dx;
            int ny = y + i * dy;
            if (nx < 0 || nx >= gridSize || ny < 0 || ny >= gridSize) return false;
            if (occupied.Contains(new Vector2Int(nx, ny)) && grid[nx, ny] != null && grid[nx, ny].letter != word[i]) return false;
        }
        return true;
    }

    private void PlaceWord(string word, int x, int y, Direction dir)
    {
        int dx = 0, dy = 0;
        switch (dir)
        {
            case Direction.Horizontal: dx = 1; break;
            case Direction.Vertical: dy = 1; break;
            case Direction.DiagonalDown: dx = 1; dy = 1; break;
            case Direction.DiagonalUp: dx = 1; dy = -1; break;
        }

        for (int i = 0; i < word.Length; i++)
        {
            int nx = x + i * dx;
            int ny = y + i * dy;
            GameObject obj = new GameObject("Cell_" + nx + "_" + ny);
            obj.transform.SetParent(gridParent);
            obj.transform.position = new Vector3(nx, -ny, 0f);
            LetterCell cell = obj.AddComponent<LetterCell>();
            if (grid[nx, ny] != null) Destroy(grid[nx, ny].gameObject);
            cell.Init(nx, ny, word[i], this);
            grid[nx, ny] = cell;
            occupied.Add(new Vector2Int(nx, ny));
        }
    }

    public bool CheckWord(List<Vector2Int> selectedCells)
    {
        char[] letters = new char[selectedCells.Count];
        for (int i = 0; i < selectedCells.Count; i++)
        {
            var pos = selectedCells[i];
            if (grid[pos.x, pos.y] != null)
                letters[i] = grid[pos.x, pos.y].letter;
        }
        string word = new string(letters);

        foreach (string w in placedWords)
        {
            if (w == word)
            {
                placedWords.Remove(w);
                foreach (var pos in selectedCells)
                {
                    if (grid[pos.x, pos.y] != null)
                        grid[pos.x, pos.y].SetFound();
                }
                GameManager.Instance.AddScore(word.Length * 10);
                return true;
            }
        }
        return false;
    }
}
