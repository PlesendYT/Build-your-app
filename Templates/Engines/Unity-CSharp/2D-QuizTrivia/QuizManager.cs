/// <summary>
/// Manages quiz trivia gameplay: question pool, shuffling, timer, scoring with speed bonus.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Data for a single trivia question with answers and difficulty.
/// </summary>
[System.Serializable]
public class QuestionData
{
    public string category;
    public string question;
    public string[] answers;
    public int correctIndex;
    public int difficulty;
}

public class QuizManager : MonoBehaviour
{
    [Header("Question Pool")]
    public List<QuestionData> allQuestions = new List<QuestionData>();
    private Queue<QuestionData> questionQueue = new Queue<QuestionData>();

    [Header("Settings")]
    public int questionsPerRound = 10;
    public float timePerQuestion = 15f;

    [Header("State")]
    private int currentScore;
    private int correctCount;
    private int questionIndex;
    private QuestionData currentQuestion;
    private float timer;
    private bool isAnswered;

    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static QuizManager Instance { get; private set; }
    public QuestionData CurrentQuestion => currentQuestion;
    public int CurrentScore => currentScore;
    public int QuestionIndex => questionIndex;
    public int TotalQuestions => questionsPerRound;
    public float Timer => timer;
    public bool IsAnswered => isAnswered;

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Starts the first round on game start.
    /// </summary>
    private void Start()
    {
        StartRound();
    }

    /// <summary>
    /// Shuffles all questions, queues them up, and starts the first question.
    /// </summary>
    public void StartRound()
    {
        currentScore = 0;
        correctCount = 0;
        questionIndex = 0;

        List<QuestionData> shuffled = new List<QuestionData>(allQuestions);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            QuestionData temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }

        questionQueue.Clear();
        int count = Mathf.Min(questionsPerRound, shuffled.Count);
        for (int i = 0; i < count; i++)
            questionQueue.Enqueue(shuffled[i]);

        NextQuestion();
    }

    /// <summary>
    /// Loads the next question or ends the round if the queue is empty.
    /// </summary>
    public void NextQuestion()
    {
        if (questionQueue.Count == 0)
        {
            EndRound();
            return;
        }

        currentQuestion = questionQueue.Dequeue();
        timer = timePerQuestion;
        isAnswered = false;
        questionIndex++;
    }

    /// <summary>
    /// Counts down the timer each frame and auto-advances on timeout.
    /// </summary>
    private void Update()
    {
        if (currentQuestion == null || isAnswered) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isAnswered = true;
            Invoke(nameof(NextQuestion), 1f);
        }
    }

    /// <summary>
    /// Submits an answer: awards score with time bonus if correct.
    /// </summary>
    public void SubmitAnswer(int answerIndex)
    {
        if (isAnswered) return;
        isAnswered = true;

        if (answerIndex == currentQuestion.correctIndex)
        {
            int bonus = Mathf.RoundToInt(timer);
            currentScore += 100 + bonus * 10;
            correctCount++;
        }

        Invoke(nameof(NextQuestion), 1.5f);
    }

    /// <summary>
    /// Logs the final score and accuracy at the end of the round.
    /// </summary>
    private void EndRound()
    {
        float percentage = (float)correctCount / questionsPerRound * 100f;
        Debug.Log("Round ended! Score: " + currentScore + " Correct: " + correctCount + "/" + questionsPerRound + " (" + percentage + "%)");
    }
}
