/// <summary>
/// Searchable loot container with randomized loot tables for extraction shooter.
/// </summary>
using UnityEngine;

public class LootContainer : MonoBehaviour
{
    [Header("Loot Settings")]
    public int minValue = 10;
    public int maxValue = 500;
    public int minWeight = 1;
    public int maxWeight = 10;
    public bool isSearched;
    public float searchTime = 2f;

    [Header("Visual")]
    public GameObject closedModel;
    public GameObject openedModel;
    public GameObject searchPrompt;

    private float searchProgress;

    private void Start()
    {
        if (closedModel != null) closedModel.SetActive(true);
        if (openedModel != null) openedModel.SetActive(false);
        if (searchPrompt != null) searchPrompt.SetActive(false);
    }

    public void StartSearch()
    {
        if (isSearched) return;
        if (searchPrompt != null) searchPrompt.SetActive(true);
    }

    public void StopSearch()
    {
        searchProgress = 0f;
        if (searchPrompt != null) searchPrompt.SetActive(false);
    }

    public bool UpdateSearch()
    {
        if (isSearched) return false;

        searchProgress += Time.deltaTime;
        if (searchProgress >= searchTime)
        {
            OpenContainer();
            return true;
        }
        return false;
    }

    private void OpenContainer()
    {
        isSearched = true;
        if (closedModel != null) closedModel.SetActive(false);
        if (openedModel != null) openedModel.SetActive(true);
        if (searchPrompt != null) searchPrompt.SetActive(false);

        // Generate random loot
        int lootValue = Random.Range(minValue, maxValue + 1);
        int lootWeight = Random.Range(minWeight, maxWeight + 1);

        // Notify player of found loot
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.AddLoot(lootValue, lootWeight);
            GameManager.Instance.AddLootValue(lootValue);
        }
    }
}
