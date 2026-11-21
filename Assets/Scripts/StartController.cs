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
                AudioManager.instance.StopMusic();
                startTextAnimation.waveSpeedMultiplier = 2;
                startTextAnimation.waveHeightMultiplier = 5;
                startTextAnimation.waveSpacingMultiplier = 2;
                LoadGame();
            }
        }
    }

    void LoadGame()
    {
        GameController.day = 0;
        StudentController.students.Clear();
        StudentController.studentSeats.Clear();

        FaderController.instance.scene = "Game";
        FaderController.instance.FadeOut(FaderController.instance.ChangeScene, 2);
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
