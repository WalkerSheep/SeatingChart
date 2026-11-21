using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FaderController : MonoBehaviour
{
    public static FaderController instance;
    public bool fading;
    [HideInInspector] public UnityEvent fadeEvent;

    private Animator animator;

    [HideInInspector] public string scene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        animator = GetComponent<Animator>();

        fading = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Fading", fading);
    }

    public void FadeOut(UnityAction unityEvent, float length = 1)
    {
        fading = true;
        fadeEvent.RemoveAllListeners();
        fadeEvent.AddListener(unityEvent);
        animator.speed = 1 / length;
    }

    public void FadeDone()
    {
        fading = false;
    }

    public void Activate()
    {
        fadeEvent.Invoke();
    }
    
    public void ChangeScene()
    {
        SceneManager.LoadScene(scene);
    }
}
