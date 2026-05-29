/// <summary>
/// Tactical shooter team-based match manager with attack/defend round system.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum Team { Attackers, Defenders }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Match")]
    public int roundsToWin = 4, attackerScore, defenderScore, currentRound = 1;
    public float roundTime = 120f, remainingTime;
    public List<PlayerController> attackers = new List<PlayerController>();
    public List<PlayerController> defenders = new List<PlayerController>();
    public Team currentSide = Team.Attackers;
    public bool isRoundActive;

    [Header("UI")]
    public TMPro.TextMeshProUGUI roundText, timerText, scoreText;
    public GameObject roundOverPanel, matchOverPanel;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start() => StartNewRound();

    private void Update()
    {
        if (!isRoundActive) return;
        remainingTime -= Time.deltaTime;
        if (roundText != null) roundText.text = "Round " + currentRound + " - " + currentSide;
        if (timerText != null) timerText.text = Mathf.FloorToInt(remainingTime / 60) + ":" + (remainingTime % 60).ToString("00");
        if (scoreText != null) scoreText.text = "A:" + attackerScore + " D:" + defenderScore;
        if (remainingTime <= 0f) EndRound(Team.Defenders);
    }

    public void StartNewRound() { isRoundActive = true; remainingTime = roundTime; if (currentRound > roundsToWin) currentSide = Team.Defenders; }

    public void EndRound(Team winner)
    {
        isRoundActive = false;
        if (winner == Team.Attackers) attackerScore++; else defenderScore++;
        if (attackerScore >= roundsToWin || defenderScore >= roundsToWin) { MatchOver(winner); return; }
        currentRound++;
        if (roundOverPanel != null) roundOverPanel.SetActive(true);
    }

    private void MatchOver(Team winner)
    {
        if (matchOverPanel != null) { matchOverPanel.SetActive(true); TMPro.TextMeshProUGUI t = matchOverPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>(); if (t != null) t.text = winner + " Win!"; }
        Time.timeScale = 0f;
    }

    public void RegisterPlayer(PlayerController p, Team t) { if (t == Team.Attackers) attackers.Add(p); else defenders.Add(p); }

    public void PlayerEliminated(PlayerController p)
    {
        attackers.Remove(p); defenders.Remove(p);
        if (isRoundActive) { if (attackers.Count == 0) EndRound(Team.Defenders); if (defenders.Count == 0) EndRound(Team.Attackers); }
    }

    public void RestartGame() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}
