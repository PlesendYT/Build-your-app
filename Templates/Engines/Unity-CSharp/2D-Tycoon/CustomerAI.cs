using UnityEngine;

public class CustomerAI : MonoBehaviour
{
    private Vector3 exitPosition;
    private CustomerSpawner spawner;
    private TycoonManager tycoon;
    private SpriteRenderer spriteRenderer;
    private float moveSpeed;
    private Vector3 target;
    private enum State { Entering, Shopping, Leaving }
    private State state = State.Entering;

    public void Init(Vector3 exit, CustomerSpawner sp)
    {
        exitPosition = exit;
        spawner = sp;
        tycoon = FindObjectOfType<TycoonManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = Random.Range(1f, 2.5f);
        tycoon = FindObjectOfType<TycoonManager>();
        target = GetRandomShoppingPoint();
    }

    private Vector3 GetRandomShoppingPoint()
    {
        return transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-1f, 1f), 0f);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Entering:
                MoveTo(target);
                break;
            case State.Shopping:
                break;
            case State.Leaving:
                MoveTo(exitPosition);
                break;
        }
    }

    private void MoveTo(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        float dist = Vector3.Distance(transform.position, destination);
        if (dist < 0.3f)
        {
            if (state == State.Entering)
            {
                state = State.Shopping;
                Invoke(nameof(StartLeaving), Random.Range(2f, 5f));
                if (tycoon != null)
                {
                    tycoon.ModifySatisfaction(1f);
                    tycoon.SpendMoney(-tycoon.basePrice);
                }
            }
            else if (state == State.Leaving)
            {
                spawner.CustomerLeft();
                Destroy(gameObject);
            }
        }
    }

    private void StartLeaving()
    {
        state = State.Leaving;
    }
}
