using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [Header("Treasure")]
    public int goldAmount = 50;
    public GameObject openEffect;
    private bool isOpened;

    public void Open()
    {
        if (isOpened) return;
        isOpened = true;

        GameManager.Instance.AddScore(goldAmount);
        if (openEffect != null)
            Instantiate(openEffect, transform.position, Quaternion.identity);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.yellow;

        PlayerCrawler player = FindObjectOfType<PlayerCrawler>();
        if (player != null)
            player.Heal(5);

        Debug.Log("Treasure found! +" + goldAmount + " gold");
    }
}
