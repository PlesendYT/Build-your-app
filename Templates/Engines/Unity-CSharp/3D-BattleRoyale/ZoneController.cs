/// <summary>
/// Shrinking safe zone with damage-over-time, phases, and minimap visualization.
/// </summary>
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [Header("Zone Settings")]
    public Vector3 center;
    public float currentRadius = 300f;
    public float finalRadius = 10f;
    public float shrinkSpeed = 2f;
    public float damagePerSecond = 5f;
    public float timeBetweenShrinks = 60f;

    private float nextShrinkTime;
    private int phase;
    private bool isShrinking;

    private void Start()
    {
        nextShrinkTime = Time.time + timeBetweenShrinks;
    }

    private void Update()
    {
        if (Time.time >= nextShrinkTime)
        {
            isShrinking = true;
        }

        if (isShrinking)
        {
            currentRadius -= shrinkSpeed * Time.deltaTime;
            transform.localScale = Vector3.one * currentRadius * 2f;

            if (currentRadius <= finalRadius)
            {
                currentRadius = finalRadius;
                isShrinking = false;
            }
        }

        DamagePlayersOutsideZone();
    }

    private void DamagePlayersOutsideZone()
    {
        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (PlayerController player in players)
        {
            float distance = Vector3.Distance(player.transform.position, center);
            if (distance > currentRadius)
            {
                player.TakeDamage(Mathf.RoundToInt(damagePerSecond * Time.deltaTime));
            }
        }
    }

    public bool IsInZone(Vector3 position)
    {
        return Vector3.Distance(position, center) <= currentRadius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawWireSphere(center, currentRadius);
    }
}
