using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        if (gameManager.GameOver)
        {
            return;
        }

        score += amount;

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}