using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    [Header("Perception")]
    public float sightRange = 15f;
    public float sightAngle = 60f;
    public float hearingRange = 10f;
    public float suspicionTime = 5f;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex;
    private NavMeshAgent agent;

    [Header("State")]
    private float suspicionTimer;
    private Vector3 lastKnownPlayerPosition;
    private Transform player;

    private enum GuardState { Patrol, Suspicious, Alert, Searching }
    private GuardState currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentState = GuardState.Patrol;
    }

    private void Start()
    {
        if (patrolPoints.Length > 0)
            agent.destination = patrolPoints[0].position;
    }

    private void Update()
    {
        if (player == null) return;

        PlayerController pc = player.GetComponent<PlayerController>();

        switch (currentState)
        {
            case GuardState.Patrol:
                Patrol();
                if (CanSeePlayer(pc)) currentState = GuardState.Alert;
                else if (CanHearPlayer(pc)) { currentState = GuardState.Suspicious; lastKnownPlayerPosition = player.position; }
                break;
            case GuardState.Suspicious:
                agent.destination = lastKnownPlayerPosition;
                suspicionTimer += Time.deltaTime;
                if (CanSeePlayer(pc)) currentState = GuardState.Alert;
                else if (suspicionTimer >= suspicionTime) currentState = GuardState.Patrol;
                break;
            case GuardState.Alert:
                agent.destination = player.position;
                AlarmSystem.Instance?.TriggerAlarm();
                if (!CanSeePlayer(pc)) currentState = GuardState.Searching;
                break;
            case GuardState.Searching:
                if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < 1f)
                    currentState = GuardState.Patrol;
                break;
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.destination = patrolPoints[currentPatrolIndex].position;
        }
    }

    private bool CanSeePlayer(PlayerController pc)
    {
        if (pc == null) return false;
        Vector3 direction = player.position - transform.position;
        if (direction.magnitude > sightRange) return false;
        if (Vector3.Angle(transform.forward, direction) > sightAngle) return false;

        float visibility = pc.GetVisibility();
        return visibility > 0.5f;
    }

    private bool CanHearPlayer(PlayerController pc)
    {
        if (pc == null) return false;
        float noise = pc.GetNoise();
        float distance = Vector3.Distance(transform.position, player.position);
        return noise > 0.3f && distance < hearingRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }
}

public class AlarmSystem : MonoBehaviour
{
    public static AlarmSystem Instance { get; private set; }
    public bool isAlarmed;
    public float alarmDuration = 30f;
    private float alarmTimer;

    private void Awake() { Instance = this; }

    public void TriggerAlarm()
    {
        if (!isAlarmed)
        {
            isAlarmed = true;
            alarmTimer = alarmDuration;
        }
    }

    private void Update()
    {
        if (isAlarmed)
        {
            alarmTimer -= Time.deltaTime;
            if (alarmTimer <= 0) isAlarmed = false;
        }
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject gameOverPanel;
    private void Awake() { if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); } else { Destroy(gameObject); } }
    public void GameOver() { if (gameOverPanel != null) gameOverPanel.SetActive(true); Time.timeScale = 0f; }
    public void RestartGame() { Time.timeScale = 1f; UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex); }
}
