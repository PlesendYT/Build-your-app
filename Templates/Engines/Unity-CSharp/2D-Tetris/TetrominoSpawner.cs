using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    [Header("Pieces")]
    public GameObject[] tetrominoPrefabs;
    public Transform spawnPosition;
    public Transform previewPosition;
    private GameObject nextPiece;

    private void Start()
    {
        nextPiece = Instantiate(GetRandomPiece(), previewPosition.position, Quaternion.identity);
        SpawnPiece();
    }

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

    private GameObject GetRandomPiece()
    {
        return tetrominoPrefabs[Random.Range(0, tetrominoPrefabs.Length)];
    }
}
