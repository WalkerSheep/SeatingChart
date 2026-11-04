using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    [HideInInspector] public bool countingDown = false;
    public float timer = 0;
    public TextMeshProUGUI timerDisplay;

    void Awake()
    {
        instance = this;

        countingDown = false;
        timer = 0;
    }

    void Update()
    {
        if(timer > 0 && countingDown)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
        }

        timerDisplay.text = timer.ToString("F2");
    }

    public void StartTimer(float startTime = 60f)
    {
        timer = startTime;
        countingDown = true;
    }

    public void PauseTimer()
    {
        countingDown = false;
    }
}
