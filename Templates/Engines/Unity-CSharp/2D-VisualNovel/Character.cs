/// <summary>
/// ScriptableObject defining a visual novel character's name, expressions, and voice lines.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Visual Novel/Character")]
public class Character : ScriptableObject
{
    public string characterName;
    public Color nameColor = Color.white;
    public Sprite neutralExpression;
    public Sprite happyExpression;
    public Sprite sadExpression;
    public Sprite angryExpression;
    public Sprite surprisedExpression;
    public AudioClip[] voiceLines;
}

/// <summary>
/// Controls the in-game display of a character portrait with expression switching.
/// </summary>
public class CharacterController : MonoBehaviour
{
    [Header("Character")]
    public Character characterData;
    public UnityEngine.UI.Image characterImage;
    public Animator animator;

    /// <summary>
    /// Sets the character portrait to the specified expression sprite.
    /// </summary>
    public void SetExpression(ExpressionType type)
    {
        Sprite expression = type switch
        {
            ExpressionType.Happy => characterData.happyExpression,
            ExpressionType.Sad => characterData.sadExpression,
            ExpressionType.Angry => characterData.angryExpression,
            ExpressionType.Surprised => characterData.surprisedExpression,
            _ => characterData.neutralExpression
        };

        if (characterImage != null) characterImage.sprite = expression;
    }

    public enum ExpressionType { Neutral, Happy, Sad, Angry, Surprised }
}
