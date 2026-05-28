using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [Header("Resource")]
    public string resourceName = "Wood";
    public int resourceAmount = 5;
    public float gatherTime = 1f;
    public GameObject gatherEffect;
    public GameObject depletedPrefab;

    public Item.ItemType resourceType = Item.ItemType.Material;

    public Item Gather()
    {
        if (resourceAmount <= 0) return null;

        resourceAmount--;

        if (gatherEffect != null)
            Instantiate(gatherEffect, transform.position, Quaternion.identity);

        Item item = new Item
        {
            itemName = resourceName,
            quantity = 1,
            type = resourceType
        };

        if (resourceAmount <= 0 && depletedPrefab != null)
        {
            Instantiate(depletedPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        return item;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TMPro.TextMeshProUGUI healthText;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void GameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
