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

    public void Initialize(int lane, Vector3 start, Vector3 target, float timeUntilBeat)
    {
        this.lane = lane;
        startPos = start;
        targetPos = target;
        travelTime = timeUntilBeat;
        lifetime = timeUntilBeat;
        transform.position = start;
    }

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

    private ScoreRating GetRating()
    {
        if (Mathf.Abs(lifetime) < 0.1f) return ScoreRating.Perfect;
        if (Mathf.Abs(lifetime) < 0.2f) return ScoreRating.Good;
        return ScoreRating.Bad;
    }

    private void Miss()
    {
        isHit = true;
        ScoreManager.Instance.AddScore(ScoreRating.Miss);
        if (spriteRenderer != null)
            spriteRenderer.color = missColor;
        Destroy(gameObject, 0.2f);
    }
}

public enum ScoreRating { Miss, Bad, Good, Perfect }
