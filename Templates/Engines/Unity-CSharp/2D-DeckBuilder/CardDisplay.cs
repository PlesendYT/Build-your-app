using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;
    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text descriptionText;
    public SpriteRenderer artworkRenderer;

    public void Init(CardData data)
    {
        cardData = data;
        if (nameText != null) nameText.text = data.cardName;
        if (costText != null) costText.text = data.cost.ToString();
        if (descriptionText != null) descriptionText.text = data.description;
        if (artworkRenderer != null && data.artwork != null) artworkRenderer.sprite = data.artwork;
    }

    private void OnMouseDown()
    {
        BattleManager battle = FindObjectOfType<BattleManager>();
        if (battle != null)
            battle.PlayCard(cardData);
    }
}
