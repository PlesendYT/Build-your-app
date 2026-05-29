/// <summary>
/// Manages the bubble shooter grid, bubble spawning, shooting, match detection, and removal of floating bubbles.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class BubbleShooterManager : MonoBehaviour
{
    [Header("Grid")]
    public int gridWidth = 12;
    public int gridHeight = 14;
    public float cellSize = 0.8f;
    public GameObject[] bubblePrefabs;
    public Transform boardParent;

    [Header("Shooting")]
    public GameObject currentBubble;
    public Transform shootPoint;
    public float shootForce = 15f;

    private Dictionary<Vector2Int, Bubble> grid = new Dictionary<Vector2Int, Bubble>();
    private int currentColorIndex;

    /// <summary>
    /// Generates the initial grid and loads the first bubble to shoot.
    /// </summary>
    private void Start()
    {
        GenerateGrid();
        LoadNextBubble();
    }

    /// <summary>
    /// Generates the hexagonal grid of bubbles with alternating row offsets and random colors.
    /// </summary>
    private void GenerateGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            int cols = (y % 2 == 0) ? gridWidth : gridWidth - 1;
            float offsetX = (y % 2 == 0) ? 0f : cellSize * 0.5f;
            for (int x = 0; x < cols; x++)
            {
                int colorIdx = Random.Range(0, bubblePrefabs.Length);
                SpawnBubble(x, y, colorIdx);
            }
        }
    }

    /// <summary>
    /// Converts a world position into grid coordinates.
    /// </summary>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        float localX = worldPos.x - boardParent.position.x;
        float localY = worldPos.y - boardParent.position.y;
        int y = Mathf.RoundToInt(localY / cellSize);
        int x = Mathf.RoundToInt((localX - (y % 2 == 0 ? 0f : cellSize * 0.5f)) / cellSize);
        return new Vector2Int(x, y);
    }

    /// <summary>
    /// Converts grid coordinates to a world position.
    /// </summary>
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        float offsetX = (gridPos.y % 2 == 0) ? 0f : cellSize * 0.5f;
        float x = boardParent.position.x + gridPos.x * cellSize + offsetX;
        float y = boardParent.position.y + gridPos.y * cellSize;
        return new Vector3(x, y, 0f);
    }

    /// <summary>
    /// Instantiates a bubble at the given grid position with the specified color.
    /// </summary>
    public void SpawnBubble(int x, int y, int colorIndex)
    {
        Vector3 pos = GridToWorld(new Vector2Int(x, y));
        GameObject obj = Instantiate(bubblePrefabs[colorIndex], pos, Quaternion.identity, boardParent);
        Bubble bubble = obj.GetComponent<Bubble>();
        bubble.Init(x, y, colorIndex, this);
        grid[new Vector2Int(x, y)] = bubble;
    }

    /// <summary>
    /// Fires the current bubble in the given direction.
    /// </summary>
    public void ShootBubble(Vector2 direction)
    {
        if (currentBubble == null) return;

        Rigidbody2D rb = currentBubble.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction * shootForce, ForceMode2D.Impulse);
        currentBubble = null;

        Invoke(nameof(LoadNextBubble), 0.5f);
    }

    /// <summary>
    /// Called when a fired bubble stops: snaps it to the grid, checks for matches of 3+, and removes floating bubbles.
    /// </summary>
    public void BubbleStopped(Bubble bubble)
    {
        Vector2Int gridPos = WorldToGrid(bubble.transform.position);
        bubble.Init(gridPos.x, gridPos.y, bubble.ColorIndex, this);
        grid[gridPos] = bubble;

        // Find all connected bubbles of the same color
        List<Bubble> connected = GetConnectedBubbles(gridPos, bubble.ColorIndex);
        if (connected.Count >= 3)
        {
            foreach (Bubble b in connected)
            {
                grid.Remove(new Vector2Int(b.GridX, b.GridY));
                Destroy(b.gameObject);
                GameManager.Instance.AddScore(10);
            }
            RemoveFloatingBubbles();
        }
    }

    /// <summary>
    /// BFS to find all connected bubbles of the same color starting from the given position.
    /// </summary>
    private List<Bubble> GetConnectedBubbles(Vector2Int pos, int color)
    {
        List<Bubble> result = new List<Bubble>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(pos);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (visited.Contains(current)) continue;
            visited.Add(current);

            if (!grid.ContainsKey(current) || grid[current].ColorIndex != color) continue;
            result.Add(grid[current]);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor) && grid.ContainsKey(neighbor) && grid[neighbor].ColorIndex == color)
                    queue.Enqueue(neighbor);
            }
        }
        return result;
    }

    /// <summary>
    /// Returns the six hexagonal neighbors for a given grid position.
    /// </summary>
    private List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        bool isOdd = pos.y % 2 != 0;
        int[] dxOdd = { -1, 0, 1, -1, 0, 1 };
        int[] dyOdd = { -1, -1, -1, 1, 1, 1 };
        int[] dxEven = { -1, 0, 1, -1, 0, 1 };
        int[] dyEven = { -1, -1, -1, 1, 1, 1 };

        int[] dx = isOdd ? dxOdd : dxEven;
        int[] dy = isOdd ? dyOdd : dyEven;

        for (int i = 0; i < 6; i++)
        {
            Vector2Int n = new Vector2Int(pos.x + dx[i], pos.y + dy[i]);
            if (n.y >= 0)
                neighbors.Add(n);
        }
        return neighbors;
    }

    /// <summary>
    /// Removes bubbles that are not connected to the ceiling (top row), making them fall with gravity.
    /// </summary>
    private void RemoveFloatingBubbles()
    {
        // BFS from the top row to find all connected bubbles
        HashSet<Vector2Int> connected = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        for (int x = 0; x < gridWidth; x++)
        {
            Vector2Int top = new Vector2Int(x, gridHeight);
            if (grid.ContainsKey(new Vector2Int(x, gridHeight)))
                queue.Enqueue(new Vector2Int(x, gridHeight));
        }

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (connected.Contains(current)) continue;
            connected.Add(current);

            foreach (Vector2Int n in GetNeighbors(current))
            {
                if (!connected.Contains(n) && grid.ContainsKey(n))
                    queue.Enqueue(n);
            }
        }

        // Remove bubbles not connected to the top
        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var kvp in grid)
        {
            if (!connected.Contains(kvp.Key))
                toRemove.Add(kvp.Key);
        }

        foreach (Vector2Int pos in toRemove)
        {
            Bubble b = grid[pos];
            grid.Remove(pos);
            if (b != null)
            {
                Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.gravityScale = 2f;
                    // Destroy after 3 seconds so they fall off screen
                    Destroy(b.gameObject, 3f);
                }
            }
        }
    }

    /// <summary>
    /// Instantiates the next bubble to be shot at the shoot point with a random color.
    /// </summary>
    private void LoadNextBubble()
    {
        currentColorIndex = Random.Range(0, bubblePrefabs.Length);
        currentBubble = Instantiate(bubblePrefabs[currentColorIndex], shootPoint.position, Quaternion.identity, boardParent);
        Bubble bubble = currentBubble.GetComponent<Bubble>();
        bubble.ColorIndex = currentColorIndex;
        Rigidbody2D rb = currentBubble.GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }
}
