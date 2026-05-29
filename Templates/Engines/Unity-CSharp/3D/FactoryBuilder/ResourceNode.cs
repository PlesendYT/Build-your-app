/// <summary>
/// Mining/extraction resource node with finite yield, depletion, and miner attachment.
/// </summary>
using UnityEngine;

public enum NodeType
{
    IronVein,
    CopperVein,
    CoalDeposit,
    OilReservoir,
    StoneDeposit
}

public class ResourceNode : MonoBehaviour
{
    [Header("Node Settings")]
    public NodeType nodeType = NodeType.IronVein;
    public string resourceName = "IronOre";
    public int totalYield = 1000;
    public int remainingYield;
    public float extractionRate = 1f; // Per second

    [Header("Miner Connection")]
    public Building connectedMiner;
    public bool isDepleted;

    [Header("Visual")]
    public GameObject fullNodeModel;
    public GameObject depletedNodeModel;
    public MeshRenderer nodeRenderer;
    public Color[] nodeColors;

    private void Start()
    {
        remainingYield = totalYield;

        // Set color based on node type
        if (nodeRenderer != null && nodeColors.Length > (int)nodeType)
        {
            nodeRenderer.material.color = nodeColors[(int)nodeType];
        }

        UpdateVisual();
    }

    public int ExtractResource(float deltaTime)
    {
        if (isDepleted || connectedMiner == null) return 0;

        // Calculate extraction amount
        float extractionAmount = extractionRate * deltaTime * connectedMiner.level;
        int amount = Mathf.FloorToInt(extractionAmount);

        // Clamp to remaining
        amount = Mathf.Min(amount, remainingYield);
        remainingYield -= amount;

        // Check depletion
        if (remainingYield <= 0)
        {
            isDepleted = true;
            UpdateVisual();
        }

        return amount;
    }

    public void ConnectMiner(Building miner)
    {
        connectedMiner = miner;
    }

    public void DisconnectMiner()
    {
        connectedMiner = null;
    }

    private void UpdateVisual()
    {
        if (fullNodeModel != null) fullNodeModel.SetActive(!isDepleted);
        if (depletedNodeModel != null) depletedNodeModel.SetActive(isDepleted);
    }

    public float GetYieldPercent()
    {
        return (float)remainingYield / totalYield;
    }

    public string GetNodeInfo()
    {
        return nodeType + " | Yield: " + remainingYield + "/" + totalYield +
               " | Miner: " + (connectedMiner != null ? "Connected" : "None");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = nodeType switch
        {
            NodeType.IronVein => new Color(0.8f, 0.4f, 0.2f),
            NodeType.CopperVein => new Color(0.8f, 0.5f, 0.2f),
            NodeType.CoalDeposit => Color.gray,
            NodeType.OilReservoir => Color.black,
            NodeType.StoneDeposit => Color.white,
            _ => Color.yellow
        };
        Gizmos.DrawSphere(transform.position + Vector3.up, 1.5f);
    }
}
