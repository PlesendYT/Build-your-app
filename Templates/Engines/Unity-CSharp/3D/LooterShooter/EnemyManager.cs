/// <summary>
/// Enemy spawning with loot table assignments and difficulty scaling for looter shooter.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LootTableEntry
{
    public string itemName;
    public GameObject itemPrefab;
    [Range(0f, 1f)] public float dropChance;
    public int minDropCount = 1;
    public int maxDropCount = 1;
}

public class EnemyManager : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject[] enemyPrefabs;
    public int maxEnemies = 20;
    public float spawnRadius = 30f;
    public float spawnInterval = 10f;
    private List<EnemyController> activeEnemies = new List<EnemyController>();

    [Header("Loot Tables")]
    public LootTableEntry[] commonLoot;
    public LootTableEntry[] rareLoot;
    public LootTableEntry[] bossLoot;

    private float nextSpawnTime;

    private void Update()
    {
        // Clean up dead enemies
        activeEnemies.RemoveAll(e => e == null);

        // Spawn new enemies up to cap
        if (Time.time >= nextSpawnTime && activeEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        // Pick random spawn position around player
        Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPos.y = 0f;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemyObj = Instantiate(prefab, spawnPos, Quaternion.identity);
        EnemyController enemy = enemyObj.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.enemyManager = this;

            // Higher level enemies drop better loot
            if (Random.value < 0.1f)
            {
                enemy.lootTable = bossLoot;
                enemy.isBoss = true;
            }
            else if (Random.value < 0.2f)
            {
                enemy.lootTable = rareLoot;
            }
            else
            {
                enemy.lootTable = commonLoot;
            }

            activeEnemies.Add(enemy);
        }
    }

    public void DropLoot(Vector3 position, LootTableEntry[] table)
    {
        foreach (LootTableEntry entry in table)
        {
            if (Random.value <= entry.dropChance && entry.itemPrefab != null)
            {
                int count = Random.Range(entry.minDropCount, entry.maxDropCount + 1);
                for (int i = 0; i < count; i++)
                {
                    Vector3 dropPos = position + Random.insideUnitSphere * 0.5f;
                    dropPos.y = 0.5f;
                    Instantiate(entry.itemPrefab, dropPos, Quaternion.identity);
                }
            }
        }
    }
}
