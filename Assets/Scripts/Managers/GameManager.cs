using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float timeDuration;

    private float currentTime;

    void Start()
    {
        timeDuration = 5f;
        currentTime = timeDuration;

        UpdateTimerDisplay();
    }

    void Update()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;

            if (currentTime < 0f)
                currentTime = 0f;

            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        timerText.text = currentTime.ToString("F2");
    }
}

