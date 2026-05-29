/// <summary>
/// Displays card data (name, cost, description, artwork) and handles playing the card on click.
/// </summary>
using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;
    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text descriptionText;
    public SpriteRenderer artworkRenderer;

    /// <summary>
    /// Initializes the card display with the given card data.
    /// </summary>
    public void Init(CardData data)
    {
        cardData = data;
        if (nameText != null) nameText.text = data.cardName;
        if (costText != null) costText.text = data.cost.ToString();
        if (descriptionText != null) descriptionText.text = data.description;
        if (artworkRenderer != null && data.artwork != null) artworkRenderer.sprite = data.artwork;
    }

    /// <summary>
    /// Plays this card through the BattleManager when clicked.
    /// </summary>
    private void OnMouseDown()
    {
        BattleManager battle = FindObjectOfType<BattleManager>();
        if (battle != null)
            battle.PlayCard(cardData);
    }
}
