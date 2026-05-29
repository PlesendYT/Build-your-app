/// <summary>
/// Combo input detection system; includes HealthBar slider and GameManager for round management.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboMove
{
    public string comboName;
    public string[] inputSequence;
    public int damage;
    public GameObject effect;
    public float timeWindow = 0.5f;
}

public class ComboSystem : MonoBehaviour
{
    public ComboMove[] combos;
    private Queue<string> inputBuffer = new Queue<string>();
    private float lastInputTime;

    public void RegisterInput(string inputName)
    {
        if (Time.time - lastInputTime > 1f)
            inputBuffer.Clear();

        inputBuffer.Enqueue(inputName);
        lastInputTime = Time.time;

        CheckCombo();
    }

    private void CheckCombo()
    {
        string[] currentSequence = inputBuffer.ToArray();

        foreach (ComboMove combo in combos)
        {
            if (currentSequence.Length < combo.inputSequence.Length) continue;

            bool match = true;
            for (int i = 0; i < combo.inputSequence.Length; i++)
            {
                if (currentSequence[currentSequence.Length - combo.inputSequence.Length + i] != combo.inputSequence[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                ExecuteCombo(combo);
                inputBuffer.Clear();
                break;
            }
        }
    }

    private void ExecuteCombo(ComboMove combo)
    {
        HealthBar healthBar = FindAnyObjectByType<HealthBar>();
        if (healthBar != null)
            healthBar.TakeDamage(combo.damage);

        if (combo.effect != null)
            Instantiate(combo.effect, transform.position + Vector3.up, Quaternion.identity);
    }
}

public class HealthBar : MonoBehaviour
{
    public UnityEngine.UI.Slider healthSlider;
    private int totalDamage;

    public void TakeDamage(int damage)
    {
        totalDamage += damage;
        if (healthSlider != null)
            healthSlider.value = 1f - (totalDamage / 100f);
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TMPro.TextMeshProUGUI roundText;
    private int playerScore;
    private int opponentScore;

    private void Awake() { Instance = this; }

    public void RoundOver()
    {
        // Handle round
    }
}
