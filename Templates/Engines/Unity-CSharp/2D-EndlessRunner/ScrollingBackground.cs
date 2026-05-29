/// <summary>
/// Creates a parallax scrolling background effect with multiple layers that loop.
/// </summary>
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [Header("Settings")]
    public float scrollSpeed = 2f;
    public Transform[] backgrounds;
    public bool isLooping = true;

    private float[] parallaxScales;
    private Vector3[] startPositions;

    /// <summary>
    /// Records start positions and calculates parallax scales (closer layers scroll faster).
    /// </summary>
    private void Start()
    {
        startPositions = new Vector3[backgrounds.Length];
        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            startPositions[i] = backgrounds[i].position;
            parallaxScales[i] = 1f / (i + 1);
        }
    }

    /// <summary>
    /// Moves each background layer leftward at different speeds and loops them when off-screen.
    /// </summary>
    private void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float offset = Time.time * scrollSpeed * parallaxScales[i];
            backgrounds[i].position = startPositions[i] + Vector3.left * offset;

            if (isLooping)
            {
                float width = backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x;
                if (backgrounds[i].position.x < startPositions[i].x - width)
                {
                    backgrounds[i].position = startPositions[i];
                }
            }
        }
    }
}
