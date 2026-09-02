using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private float timeDuration = 5f;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    private float currentTime;

    public bool GameOver { get; private set; }

    private void Start()
    {
        currentTime = timeDuration;
        GameOver = false;

        gameOverText.gameObject.SetActive(false);

        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (GameOver)
        {
            return;
        }

        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                UpdateTimerDisplay();

                PlayerGameOver();
            }
            else
            {
                UpdateTimerDisplay();
            }
        }
    }

    public void PlayerGameOver()
    {
        GameOver = true;

        gameOverText.gameObject.SetActive(true);

        // Disable player control
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Slow the game down
        Time.timeScale = 0.2f;
    }

    private void UpdateTimerDisplay()
    {
        timerText.text = currentTime.ToString("F2");
    }
}