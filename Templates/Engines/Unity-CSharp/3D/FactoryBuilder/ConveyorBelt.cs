/// <summary>
/// Conveyor belt for item transport between buildings with speed and item buffering.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour
{
    [Header("Belt Settings")]
    public float beltSpeed = 2f, itemSpacing = 1f;
    public int maxItems = 10;

    [Header("Connections")]
    public ConveyorBelt inputBelt, outputBelt;
    public Building outputBuilding;

    [Header("Visual")]
    public Transform[] beltPoints;

    private Queue<GameObject> items = new Queue<GameObject>();
    private List<float> itemPositions = new List<float>();

    private void Update()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            itemPositions[i] += beltSpeed * Time.deltaTime;
            float len = GetBeltLength();
            float t = Mathf.Clamp01(itemPositions[i] / len);
            int idx = Mathf.Clamp(Mathf.FloorToInt(t * (beltPoints.Length - 1)), 0, beltPoints.Length - 1);
            if (items.ToArray()[i] != null) items.ToArray()[i].transform.position = beltPoints[idx].position;
            if (itemPositions[i] >= len) { items.Dequeue(); itemPositions.RemoveAt(i); }
        }

        if (items.Count > 0 && itemPositions[0] >= GetBeltLength())
        {
            GameObject item = items.Dequeue(); itemPositions.RemoveAt(0);
            if (outputBelt != null && outputBelt.CanAcceptItem()) outputBelt.AddItem(item);
            else if (outputBuilding != null) outputBuilding.ReceiveItem(item);
        }
    }

    public bool CanAcceptItem() => items.Count < maxItems;
    public void AddItem(GameObject item) { if (!CanAcceptItem()) return; items.Enqueue(item); itemPositions.Add(0f); if (beltPoints.Length > 0) item.transform.position = beltPoints[0].position; }
    private float GetBeltLength() { float len = 0f; for (int i = 1; i < beltPoints.Length; i++) len += Vector3.Distance(beltPoints[i - 1].position, beltPoints[i].position); return len > 0f ? len : 5f; }
}
