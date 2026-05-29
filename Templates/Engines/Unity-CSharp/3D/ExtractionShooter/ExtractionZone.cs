/// <summary>
/// Zone-based extraction point where players extract with loot to end the raid.
/// </summary>
using UnityEngine;

public class ExtractionZone : MonoBehaviour
{
    [Header("Extraction")]
    public string zoneName = "Extraction Point A";
    public float extractionTime = 5f;
    public float activationRadius = 3f;
    public bool isActive = true;

    [Header("Visual")]
    public Light zoneLight;
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.red;
    public ParticleSystem extractionEffect;

    private void Start()
    {
        if (zoneLight != null) zoneLight.color = activeColor;
        if (extractionEffect != null) extractionEffect.Stop();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isActive ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }

    public void OnPlayerEnter(PlayerController player)
    {
        if (!isActive) return;
        StartCoroutine(ExtractPlayer(player));
    }

    private System.Collections.IEnumerator ExtractPlayer(PlayerController player)
    {
        // Play extraction visual effects
        if (extractionEffect != null) extractionEffect.Play();
        if (zoneLight != null) zoneLight.color = inactiveColor;

        // Wait for extraction timer
        float timer = 0f;
        while (timer < extractionTime)
        {
            // Cancel extraction if player leaves radius
            if (Vector3.Distance(transform.position, player.transform.position) > activationRadius)
            {
                if (extractionEffect != null) extractionEffect.Stop();
                if (zoneLight != null) zoneLight.color = activeColor;
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Successful extraction
        GameManager.Instance.AddLootValue(player.carriedLootValue);
        GameManager.Instance.PlayerExtracted();
        player.gameObject.SetActive(false);
    }
}
