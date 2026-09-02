using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text gameOver;
    [SerializeField] private float timeDuration = 5f;

    private float currentTime;

    void Start()
    {
        currentTime = timeDuration;

        gameOver.gameObject.SetActive(false);

        UpdateTimerDisplay();
    }

    void Update()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                UpdateTimerDisplay();

                gameOver.gameObject.SetActive(true);

                // Slow the game down
                Time.timeScale = 0.2f;
            }
            else
            {
                UpdateTimerDisplay();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        timerText.text = currentTime.ToString("F2");
    }
}

