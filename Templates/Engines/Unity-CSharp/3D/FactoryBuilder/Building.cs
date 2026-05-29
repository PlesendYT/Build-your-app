/// <summary>
/// Building placement and production: miners, smelters, assemblers with recipes and power usage.
/// </summary>
using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Building Type")]
    public BuildingType buildingType = BuildingType.Miner;
    public string buildingName = "Miner MK1";
    public int level = 1;

    [Header("Production")]
    public string inputResource, outputResource;
    public float productionTime = 2f, powerUsage = 10f;
    public int inputPerCycle = 1, outputPerCycle = 1;

    [Header("State")]
    public bool isPowered, isProducing;
    public float productionProgress;
    public int storedInput, maxInputStorage = 10, storedOutput, maxOutputStorage = 10;

    [Header("Visual")]
    public GameObject activeVisual, idleVisual;
    public ParticleSystem productionEffect;

    public enum BuildingType { Miner, Smelter, Assembler, Constructor, Storage, PowerGenerator }

    private void Start() { GameManager.Instance.RegisterBuilding(this); UpdateVisual(); }

    private void Update()
    {
        if (!isPowered || (inputResource.Length > 0 && storedInput < inputPerCycle) || storedOutput + outputPerCycle > maxOutputStorage) return;
        productionProgress += Time.deltaTime;
        if (productionProgress >= productionTime)
        {
            if (inputResource.Length > 0) storedInput -= inputPerCycle;
            storedOutput += outputPerCycle;
            GameManager.Instance.AddResource(outputResource, outputPerCycle);
            if (productionEffect != null) productionEffect.Play();
            productionProgress = 0f;
        }
    }

    public void ReceiveItem(GameObject item) { if (storedInput < maxInputStorage) { storedInput++; Destroy(item); } }
    public void SetPowered(bool p) { isPowered = p; UpdateVisual(); }
    private void UpdateVisual() { if (activeVisual != null) activeVisual.SetActive(isPowered && isProducing); if (idleVisual != null) idleVisual.SetActive(!isPowered || !isProducing); }
}
