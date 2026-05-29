/// <summary>
/// A rhythm game note that travels from a spawn point to a target, with hit detection and scoring.
/// </summary>
using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color perfectColor = Color.green;
    public Color goodColor = Color.yellow;
    public Color missColor = Color.red;

    private int lane;
    private float travelTime;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isHit;
    private float lifetime;

    /// <summary>
    /// Sets the note's lane, start/target positions, and travel duration.
    /// </summary>
    public void Initialize(int lane, Vector3 start, Vector3 target, float timeUntilBeat)
    {
        this.lane = lane;
        startPos = start;
        targetPos = target;
        travelTime = timeUntilBeat;
        lifetime = timeUntilBeat;
        transform.position = start;
    }

    /// <summary>
    /// Moves the note toward its target and triggers a miss if it passes the window.
    /// </summary>
    private void Update()
    {
        if (isHit) return;

        lifetime -= Time.deltaTime;
        float t = 1f - (lifetime / travelTime);
        transform.position = Vector3.Lerp(startPos, targetPos, t);

        if (lifetime < -0.5f)
        {
            Miss();
        }
    }

    /// <summary>
    /// Attempts to hit the note within the precision window, returning true on success.
    /// </summary>
    public bool TryHit(float precisionWindow)
    {
        if (isHit) return false;

        if (Mathf.Abs(lifetime) <= precisionWindow)
        {
            isHit = true;
            ScoreRating rating = GetRating();
            ScoreManager.Instance.AddScore(rating);
            if (spriteRenderer != null)
                spriteRenderer.color = rating == ScoreRating.Perfect ? perfectColor : goodColor;
            Destroy(gameObject, 0.2f);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Determines the accuracy rating based on how close the hit was to the beat.
    /// </summary>
    private ScoreRating GetRating()
    {
        if (Mathf.Abs(lifetime) < 0.1f) return ScoreRating.Perfect;
        if (Mathf.Abs(lifetime) < 0.2f) return ScoreRating.Good;
        return ScoreRating.Bad;
    }

    /// <summary>
    /// Handles a miss: adds miss score, changes color, and destroys the note.
    /// </summary>
    private void Miss()
    {
        isHit = true;
        ScoreManager.Instance.AddScore(ScoreRating.Miss);
        if (spriteRenderer != null)
            spriteRenderer.color = missColor;
        Destroy(gameObject, 0.2f);
    }
}

/// <summary>
/// Accuracy rating for rhythm game note hits.
/// </summary>
public enum ScoreRating { Miss, Bad, Good, Perfect }
