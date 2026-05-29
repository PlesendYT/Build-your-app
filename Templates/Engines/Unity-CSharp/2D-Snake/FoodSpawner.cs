/// <summary>
/// Spawns food at random grid-aligned positions for the snake to eat.
/// </summary>
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("Food")]
    public GameObject foodPrefab;
    public Vector2 gridSize = new Vector2(18, 10);
    public Vector2 gridOffset;

    /// <summary>
    /// Spawns food at a random grid-aligned position within the bounds.
    /// </summary>
    public void SpawnFood()
    {
        float x = Mathf.Round(Random.Range(-gridSize.x / 2f, gridSize.x / 2f) + gridOffset.x);
        float y = Mathf.Round(Random.Range(-gridSize.y / 2f, gridSize.y / 2f) + gridOffset.y);
        Instantiate(foodPrefab, new Vector3(x, y, 0f), Quaternion.identity);
    }

    /// <summary>
    /// Spawns the first piece of food on start.
    /// </summary>
    private void Start()
    {
        SpawnFood();
    }
}
