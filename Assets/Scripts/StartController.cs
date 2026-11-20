using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    public static StartController instance;
    public TextMeshProUGUI highScoreTextbox;

    public bool pressed;

    public InputActionReference confirm;

    void Start()
    {
        pressed = false;
        GameController.day = 0;
        StudentController.students.Clear();
        StudentController.studentSeats.Clear();
        highScoreTextbox.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
    }

    void Update()
    {
        if(!pressed)
        {
            if(confirm.action.WasPressedThisFrame())
            {
                pressed = true;
                SceneManager.LoadScene("Game");
            }
        }
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
