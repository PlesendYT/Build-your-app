using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject customerPrefab;
    public float spawnInterval = 3f;
    public int maxCustomers = 10;
    public Transform spawnPoint;
    public Transform exitPoint;
    private float spawnTimer;
    private int activeCustomers;

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f && activeCustomers < maxCustomers)
        {
            SpawnCustomer();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnCustomer()
    {
        GameObject customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        CustomerAI ai = customer.GetComponent<CustomerAI>();
        if (ai != null)
        {
            ai.Init(exitPoint.position, this);
        }
        activeCustomers++;
    }

    public void CustomerLeft()
    {
        activeCustomers = Mathf.Max(0, activeCustomers - 1);
    }
}
