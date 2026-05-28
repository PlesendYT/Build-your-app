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

    private void Start()
    {
        grid = new Tile[width, height];
        GenerateBoard();
    }

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

    private int GetRandomType()
    {
        return Random.Range(0, tilePrefabs.Length);
    }

    private void SpawnTile(int x, int y, int type)
    {
        Vector3 pos = new Vector3(x * cellSize, y * cellSize, 0f) + boardParent.position;
        GameObject obj = Instantiate(tilePrefabs[type], pos, Quaternion.identity, boardParent);
        Tile tile = obj.GetComponent<Tile>();
        tile.Init(x, y, type);
        grid[x, y] = tile;
    }

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

    private bool IsAdjacent(Tile a, Tile b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

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
            grid[ax, ay] = a;
            grid[bx, by] = b;
            a.MoveTo(ax, ay);
            b.MoveTo(bx, by);
        }

        isProcessing = false;
    }

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
