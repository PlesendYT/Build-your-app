/// <summary>
/// Manages the match-three board: grid generation, tile selection, swapping, match detection, cascading, and refilling.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 8;
    public int height = 8;
    public float cellSize = 1f;
    public GameObject[] tilePrefabs;
    private Tile[,] grid;

    [Header("References")]
    public Transform boardParent;

    private Tile selectedTile;
    private bool isProcessing;

    /// <summary>
    /// Initializes the grid and generates the board.
    /// </summary>
    private void Start()
    {
        grid = new Tile[width, height];
        GenerateBoard();
    }

    /// <summary>
    /// Fills the board with random tiles and resolves any initial matches to ensure no pre-existing matches.
    /// </summary>
    private void GenerateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnTile(x, y, GetRandomType());
            }
        }

        while (HasInitialMatches())
        {
            ResolveInitialMatches();
        }
    }

    /// <summary>
    /// Returns a random tile type index.
    /// </summary>
    private int GetRandomType()
    {
        return Random.Range(0, tilePrefabs.Length);
    }

    /// <summary>
    /// Creates a tile at the specified grid position.
    /// </summary>
    private void SpawnTile(int x, int y, int type)
    {
        Vector3 pos = new Vector3(x * cellSize, y * cellSize, 0f) + boardParent.position;
        GameObject obj = Instantiate(tilePrefabs[type], pos, Quaternion.identity, boardParent);
        Tile tile = obj.GetComponent<Tile>();
        tile.Init(x, y, type);
        grid[x, y] = tile;
    }

    /// <summary>
    /// Checks if the board has any matches of three or more in a row.
    /// </summary>
    private bool HasInitialMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x >= 2 && grid[x, y].type == grid[x - 1, y].type && grid[x, y].type == grid[x - 2, y].type)
                    return true;
                if (y >= 2 && grid[x, y].type == grid[x, y - 1].type && grid[x, y].type == grid[x, y - 2].type)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Replaces tiles that are part of initial matches with a different random type.
    /// </summary>
    private void ResolveInitialMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x >= 2 && grid[x, y].type == grid[x - 1, y].type && grid[x, y].type == grid[x - 2, y].type)
                {
                    int newType = (grid[x, y].type + 1) % tilePrefabs.Length;
                    Destroy(grid[x, y].gameObject);
                    SpawnTile(x, y, newType);
                }
                if (y >= 2 && grid[x, y].type == grid[x, y - 1].type && grid[x, y].type == grid[x, y - 2].type)
                {
                    int newType = (grid[x, y].type + 1) % tilePrefabs.Length;
                    Destroy(grid[x, y].gameObject);
                    SpawnTile(x, y, newType);
                }
            }
        }
    }

    /// <summary>
    /// Handles tile selection: select first tile, select adjacent tile to swap, or deselect.
    /// </summary>
    public void SelectTile(Tile tile)
    {
        if (isProcessing) return;

        if (selectedTile == null)
        {
            selectedTile = tile;
            tile.SetSelected(true);
        }
        else if (selectedTile == tile)
        {
            selectedTile.SetSelected(false);
            selectedTile = null;
        }
        else if (IsAdjacent(selectedTile, tile))
        {
            StartCoroutine(SwapTiles(selectedTile, tile));
        }
        else
        {
            selectedTile.SetSelected(false);
            selectedTile = tile;
            tile.SetSelected(true);
        }
    }

    /// <summary>
    /// Checks if two tiles are adjacent horizontally or vertically.
    /// </summary>
    private bool IsAdjacent(Tile a, Tile b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

    /// <summary>
    /// Swaps two tiles, checks for matches, and reverts the swap if no match is found.
    /// </summary>
    private System.Collections.IEnumerator SwapTiles(Tile a, Tile b)
    {
        isProcessing = true;
        selectedTile.SetSelected(false);
        selectedTile = null;

        int ax = a.x, ay = a.y;
        int bx = b.x, by = b.y;

        grid[ax, ay] = b;
        grid[bx, by] = a;
        a.MoveTo(bx, by);
        b.MoveTo(ax, ay);

        yield return new WaitForSeconds(0.3f);

        List<Tile> matches = FindMatches();
        if (matches.Count > 0)
        {
            yield return StartCoroutine(ProcessMatches(matches));
        }
        else
        {
            // Swap back if no match
            grid[ax, ay] = a;
            grid[bx, by] = b;
            a.MoveTo(ax, ay);
            b.MoveTo(bx, by);
        }

        isProcessing = false;
    }

    /// <summary>
    /// Scans the board for horizontal and vertical matches of three or more.
    /// </summary>
    private List<Tile> FindMatches()
    {
        List<Tile> matches = new List<Tile>();
        bool[,] visited = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x >= 2 && grid[x, y].type == grid[x - 1, y].type && grid[x, y].type == grid[x - 2, y].type)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (!visited[x - i, y])
                        {
                            matches.Add(grid[x - i, y]);
                            visited[x - i, y] = true;
                        }
                    }
                }
                if (y >= 2 && grid[x, y].type == grid[x, y - 1].type && grid[x, y].type == grid[x, y - 2].type)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (!visited[x, y - i])
                        {
                            matches.Add(grid[x, y - i]);
                            visited[x, y - i] = true;
                        }
                    }
                }
            }
        }
        return matches;
    }

    /// <summary>
    /// Destroys matched tiles, collapses the board, fills empty spaces, and recursively processes chain matches.
    /// </summary>
    private System.Collections.IEnumerator ProcessMatches(List<Tile> matches)
    {
        foreach (Tile t in matches)
        {
            GameManager.Instance.AddScore(10);
            Destroy(t.gameObject);
            grid[t.x, t.y] = null;
        }

        yield return new WaitForSeconds(0.2f);
        CollapseBoard();
        yield return new WaitForSeconds(0.3f);
        FillEmptySpaces();

        List<Tile> newMatches = FindMatches();
        if (newMatches.Count > 0)
            yield return StartCoroutine(ProcessMatches(newMatches));
    }

    /// <summary>
    /// Shifts tiles downward to fill gaps left by destroyed tiles.
    /// </summary>
    private void CollapseBoard()
    {
        for (int x = 0; x < width; x++)
        {
            int emptyBelow = 0;
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    emptyBelow++;
                }
                else if (emptyBelow > 0)
                {
                    grid[x, y - emptyBelow] = grid[x, y];
                    grid[x, y - emptyBelow].MoveTo(x, y - emptyBelow);
                    grid[x, y] = null;
                }
            }
        }
    }

    /// <summary>
    /// Spawns new random tiles in any remaining empty grid cells.
    /// </summary>
    private void FillEmptySpaces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    SpawnTile(x, y, GetRandomType());
                }
            }
        }
    }
}
