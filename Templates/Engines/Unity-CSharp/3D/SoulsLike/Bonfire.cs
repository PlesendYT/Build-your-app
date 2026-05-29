/// <summary>
/// Bonfire checkpoint system for souls-like respawn, healing, and level progression.
/// </summary>
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [Header("Bonfire Settings")]
    public string bonfireName = "Unnamed Bonfire";
    public bool isLit;
    public float interactionRadius = 3f;
    public float healAmount = 100f;

    [Header("Visual")]
    public ParticleSystem fireEffect;
    public Light fireLight;
    public GameObject promptUI;
    public Color unlitColor = Color.gray;
    public Color litColor = Color.yellow;

    private void Start()
    {
        if (fireEffect != null) fireEffect.Stop();
        if (fireLight != null) fireLight.color = unlitLightColor;
        if (promptUI != null) promptUI.SetActive(false);
    }

    private Color unlitLightColor;

    private void Awake()
    {
        unlitLightColor = fireLight != null ? fireLight.color : Color.gray;
    }

    private void Update()
    {
        // Show prompt when player is near
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (promptUI != null)
                promptUI.SetActive(dist <= interactionRadius && !isLit);
        }
    }

    public void Interact()
    {
        if (isLit)
        {
            RestAtBonfire();
        }
        else
        {
            LightBonfire();
        }
    }

    private void LightBonfire()
    {
        isLit = true;

        // Visual feedback
        if (fireEffect != null) fireEffect.Play();
        if (fireLight != null) fireLight.color = litColor;

        // Notify game manager
        GameManager.Instance.LightBonfire(transform);
    }

    private void RestAtBonfire()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            // Fully heal and restore stamina
            player.RestoreStats();
        }

        // Reset nearby enemies
        Collider[] nearby = Physics.OverlapSphere(transform.position, 20f);
        foreach (Collider col in nearby)
        {
            EnemyAI enemy = col.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        // Log rest event
        Debug.Log("Restored at " + bonfireName);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isLit ? Color.yellow : Color.gray;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
