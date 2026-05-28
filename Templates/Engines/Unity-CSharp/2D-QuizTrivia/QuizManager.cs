using UnityEngine;
using System.Collections.Generic;

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

    public static QuizManager Instance { get; private set; }
    public QuestionData CurrentQuestion => currentQuestion;
    public int CurrentScore => currentScore;
    public int QuestionIndex => questionIndex;
    public int TotalQuestions => questionsPerRound;
    public float Timer => timer;
    public bool IsAnswered => isAnswered;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartRound();
    }

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

    private void EndRound()
    {
        float percentage = (float)correctCount / questionsPerRound * 100f;
        Debug.Log("Round ended! Score: " + currentScore + " Correct: " + correctCount + "/" + questionsPerRound + " (" + percentage + "%)");
    }
}
