using UnityEngine;
using TMPro;

public class QuestionDisplay : MonoBehaviour
{
    public TMP_Text questionText;
    public TMP_Text[] answerButtons;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text progressText;

    private void Update()
    {
        QuizManager qm = QuizManager.Instance;
        if (qm == null) return;

        if (qm.CurrentQuestion != null)
        {
            questionText.text = qm.CurrentQuestion.question;
            for (int i = 0; i < answerButtons.Length && i < qm.CurrentQuestion.answers.Length; i++)
                answerButtons[i].text = qm.CurrentQuestion.answers[i];
        }

        scoreText.text = "Score: " + qm.CurrentScore;
        timerText.text = "Time: " + Mathf.Ceil(qm.Timer);
        progressText.text = "Question " + qm.QuestionIndex + "/" + qm.TotalQuestions;
    }

    public void OnAnswerClicked(int index)
    {
        QuizManager.Instance.SubmitAnswer(index);
    }
}
