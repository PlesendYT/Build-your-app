/// <summary>
/// An XP gem dropped by enemies that the player collects by touching.
/// </summary>
using UnityEngine;

public class XPGem : MonoBehaviour
{
    [Header("XP")]
    public int xpAmount = 10;

    [Header("Visual")]
    public float magnetSpeed = 8f;
    public float magnetRange = 3f;
    private Transform player;

    /// <summary>
    /// Finds the player for magnetic attraction.
    /// </summary>
    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

        // Auto-destroy after 15 seconds if not collected
        Destroy(gameObject, 15f);
    }

    /// <summary>
    /// Moves toward the player when within magnet range.
    /// </summary>
    private void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= magnetRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, player.position, magnetSpeed * Time.deltaTime);
        }
    }
}
