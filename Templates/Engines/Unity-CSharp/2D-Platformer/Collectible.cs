using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Settings")]
    public int scoreValue = 100;
    public bool isPowerUp = false;
    public float rotationSpeed = 100f;

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isPowerUp)
            {
                GameManager.Instance.AddScore(scoreValue * 2);
            }
            else
            {
                GameManager.Instance.AddScore(scoreValue);
            }
            Destroy(gameObject);
        }
    }
}
