using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public enum GameState{ChangingChart, ViewingStudents}
    public GameState gameState;
    public bool changingChart => gameState == GameState.ChangingChart;
    public bool viewingStudents => gameState == GameState.ViewingStudents;

    public InputActionReference confirm;
    public GameObject changingChartHolder;

    void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        changingChartHolder.gameObject.SetActive(true);
        if(StudentController.students.Count == 0)
        {
            StudentController.instance.GenerateStudents();
        }
        StudentController.instance.PlaceStudents();
        yield return new WaitUntil(() => confirm.action.WasPressedThisFrame());
        gameState = GameState.ViewingStudents;
        changingChartHolder.gameObject.SetActive(false);

        TimerController.instance.StartTimer();
    }

    void OnEnable()
    {
        confirm.action.Enable();
    }

    void OnDisable()
    {
        confirm.action.Disable();
    }
}
