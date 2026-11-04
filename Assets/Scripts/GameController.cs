using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(StudentController.students.Count == 0)
        {
            StudentController.instance.GenerateStudents();
        }
        TimerController.instance.StartTimer();
    }
}
