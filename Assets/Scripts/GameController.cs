using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public enum GameState{PlayingIntro, ChangingChart, ViewingStudents, PlayingOutro}
    public GameState gameState;
    public bool changingChart => gameState == GameState.ChangingChart;
    public bool viewingStudents => gameState == GameState.ViewingStudents;

    public InputActionReference confirm;
    public InputActionReference start;
    public GameObject changingChartHolder;
    public static int day = 0;
    public GameObject dayScreen;
    public TextMeshProUGUI dayTextbox;
    public TextMeshProUGUI dayCounterTextbox;

    void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        gameState = GameState.PlayingIntro;
        day++;
        dayTextbox.text = "Day " + day;
        dayCounterTextbox.text = "Day " + day;
        dayScreen.SetActive(true);
        changingChartHolder.gameObject.SetActive(true);
        if(StudentController.students.Count == 0)
        {
            StudentController.instance.GenerateStudents();
        }
        StudentController.instance.PlaceStudents();
        yield return new WaitForSecondsRealtime(2);
        dayScreen.SetActive(false);
        gameState = GameState.ChangingChart;
        yield return new WaitUntil(() => !start.action.IsPressed());
        yield return new WaitUntil(() => start.action.WasPressedThisFrame());
        gameState = GameState.ViewingStudents;
        SeatingController.instance.hover = null;
        changingChartHolder.gameObject.SetActive(false);

        TimerController.instance.StartTimer();
    }

    void OnEnable()
    {
        confirm.action.Enable();
        start.action.Enable();
    }

    void OnDisable()
    {
        confirm.action.Disable();
        start.action.Disable();
    }
}
