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
        yield return new WaitUntil(() => (Gamepad.current == null && confirm.action.WasPressedThisFrame()) || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame));
        gameState = GameState.ViewingStudents;
        SeatingController.instance.hover = null;
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
