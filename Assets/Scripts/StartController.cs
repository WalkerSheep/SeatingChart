using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    public static StartController instance;
    public TextMeshProUGUI highScoreTextbox;

    public bool pressed;

    public TextAnimation startTextAnimation;

    public InputActionReference confirm;

    void Start()
    {
        pressed = false;
        highScoreTextbox.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
    }

    void Update()
    {
        if(!pressed)
        {
            if(confirm.action.WasPressedThisFrame())
            {
                pressed = true;
                startTextAnimation.waveSpeedMultiplier = 2;
                startTextAnimation.waveHeightMultiplier = 5;
                startTextAnimation.waveSpacingMultiplier = 2;
                Invoke(nameof(LoadGame), 1);
            }
        }
    }

    void LoadGame()
    {
        GameController.day = 0;
        StudentController.students.Clear();
        StudentController.studentSeats.Clear();

        SceneManager.LoadScene("Game");
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
