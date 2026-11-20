
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    [HideInInspector] public bool countingDown = false;
    public float timer = 0;
    public TextMeshProUGUI timerDisplay;

    public bool done;

    void Awake()
    {
        instance = this;

        countingDown = false;
        timer = 0;
        done = false;
    }

    void Update()
    {
        if(timer > 0 && countingDown)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                timer = 0;
                countingDown = false;
                done = true;

                SceneManager.LoadScene("Game");
            }
        }

        timerDisplay.text = timer.ToString("F2");
    }

    public void StartTimer(float startTime = 30f)
    {
        timer = startTime;
        countingDown = true;
        done = false;
    }

    public void PauseTimer()
    {
        countingDown = false;
    }
}
