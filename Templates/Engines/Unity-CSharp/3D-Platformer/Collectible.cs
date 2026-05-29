/// <summary>
/// Rotating collectible that grants score on trigger and spawns effects.
/// </summary>
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Settings")]
    public int scoreValue = 100;
    public GameObject pickupEffect;
    public AudioClip pickupSound;
    public bool rotate = true;
    public float rotationSpeed = 90f;

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isPowerUp)
            {
                other.GetComponent<PlayerController>().UnlockDoubleJump();
            }
            else
            {
                GameManager.Instance.AddScore(scoreValue);
            }

            if (collectEffect != null)
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
