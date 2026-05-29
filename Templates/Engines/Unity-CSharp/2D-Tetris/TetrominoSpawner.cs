/// <summary>
/// Spawns random Tetris tetromino pieces with a next-piece preview system.
/// </summary>
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    [Header("Pieces")]
    public GameObject[] tetrominoPrefabs;
    public Transform spawnPosition;
    public Transform previewPosition;
    private GameObject nextPiece;

    /// <summary>
    /// Creates the first preview piece and spawns the first active piece.
    /// </summary>
    private void Start()
    {
        nextPiece = Instantiate(GetRandomPiece(), previewPosition.position, Quaternion.identity);
        SpawnPiece();
    }

    /// <summary>
    /// Moves the preview piece to the spawn position, enables its controller, and generates a new preview.
    /// </summary>
    public void SpawnPiece()
    {
        if (nextPiece != null)
        {
            GameObject current = nextPiece;
            current.transform.position = spawnPosition.position;
            current.GetComponent<TetrominoController>().enabled = true;
        }
        nextPiece = Instantiate(GetRandomPiece(), previewPosition.position, Quaternion.identity);
        nextPiece.GetComponent<TetrominoController>().enabled = false;
    }

    /// <summary>
    /// Returns a random tetromino prefab from the pool.
    /// </summary>
    private GameObject GetRandomPiece()
    {
        return tetrominoPrefabs[Random.Range(0, tetrominoPrefabs.Length)];
    }
}
