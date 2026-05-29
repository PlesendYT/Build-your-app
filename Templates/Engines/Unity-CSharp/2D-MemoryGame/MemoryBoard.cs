/// <summary>
/// Manages the memory card board: generates shuffled card pairs, handles selection, and checks for matches.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class MemoryBoard : MonoBehaviour
{
    [Header("Grid Settings")]
    public int rows = 4;
    public int cols = 4;
    public GameObject cardPrefab;
    public Sprite[] cardSprites;
    public Transform boardParent;
    public float spacing = 1.5f;

    private List<Card> cards = new List<Card>();
    private Card firstSelected;
    private Card secondSelected;
    private bool isProcessing;
    private int matchedPairs;

    /// <summary>
    /// Generates the shuffled card grid on start.
    /// </summary>
    private void Start()
    {
        GenerateBoard();
    }

    /// <summary>
    /// Creates a grid of paired cards with shuffled sprite assignments.
    /// </summary>
    private void GenerateBoard()
    {
        int totalCards = rows * cols;
        if (totalCards % 2 != 0) totalCards++;

        // Create pairs of sprite indices
        List<int> spriteIndexes = new List<int>();
        for (int i = 0; i < totalCards / 2; i++)
        {
            spriteIndexes.Add(i % cardSprites.Length);
            spriteIndexes.Add(i % cardSprites.Length);
        }

        // Shuffle the sprite indices
        for (int i = spriteIndexes.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = spriteIndexes[i];
            spriteIndexes[i] = spriteIndexes[j];
            spriteIndexes[j] = temp;
        }

        float startX = -(cols - 1) * spacing / 2f;
        float startY = (rows - 1) * spacing / 2f;

        int idx = 0;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Vector3 pos = new Vector3(startX + x * spacing, startY - y * spacing, 0f);
                GameObject obj = Instantiate(cardPrefab, pos, Quaternion.identity, boardParent);
                Card card = obj.GetComponent<Card>();
                int spriteIdx = spriteIndexes[idx];
                card.Init(spriteIdx, cardSprites[spriteIdx], this);
                cards.Add(card);
                idx++;
            }
        }
    }

    /// <summary>
    /// Handles card click: reveals the card, and checks for a match if two cards are selected.
    /// </summary>
    public void CardClicked(Card card)
    {
        if (isProcessing || card.IsRevealed || card.IsMatched) return;

        card.Reveal();

        if (firstSelected == null)
        {
            firstSelected = card;
        }
        else
        {
            secondSelected = card;
            isProcessing = true;
            Invoke(nameof(CheckMatch), 0.8f);
        }
    }

    /// <summary>
    /// Compares the two selected cards: marks as matched if same, or hides them after a delay.
    /// </summary>
    private void CheckMatch()
    {
        if (firstSelected.SpriteIndex == secondSelected.SpriteIndex)
        {
            firstSelected.SetMatched();
            secondSelected.SetMatched();
            matchedPairs++;
            GameManager.Instance.AddScore(100);

            if (matchedPairs >= cards.Count / 2)
                GameManager.Instance.GameOver();
        }
        else
        {
            firstSelected.Hide();
            secondSelected.Hide();
        }

        firstSelected = null;
        secondSelected = null;
        isProcessing = false;
    }
}
