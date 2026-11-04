using System.Collections;
using TMPro;
using UnityEngine;

public class StudentObject : MonoBehaviour, IInteractable
{
    private Animator animator;
    [System.NonSerialized]
    public Student data;

    public SpriteRenderer bodyRenderer, faceRenderer, hatRenderer;
    public TextMeshProUGUI nameTag;

    public bool talking;
    private float talkTimer = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(talkTimer > 0)
        {
            talkTimer -= Time.deltaTime;
        }
        else
        {
            talkTimer = data.shyness;
        }

        if(transform.parent != null)
        {
            transform.position = Vector3.Lerp(transform.position, transform.parent.position, 5 * Time.deltaTime);
        }
    }

    public void Initialize(Student data)
    {
        this.data = data;
        
        nameTag.text = data.name;
        bodyRenderer.sprite = data.body;
        faceRenderer.sprite = data.face;
        if(data.hat)
        {
            hatRenderer.gameObject.SetActive(true);
            hatRenderer.sprite = data.hat;
        }
        bodyRenderer.color = data.bodyColor;

        talkTimer = data.shyness;
    }

    public void OnMouseEnter()
    {
        
    }

    public void OnMouseExit()
    {
        
    }

    public void OnMouseClick()
    {
        Debug.Log(data.name);
        if(GameController.instance.changingChart)
        {
            if(SeatingController.instance.selectedStudent == null)
            {
                SeatingController.instance.selectedStudent = this;
            }
            else if(SeatingController.instance.selectedStudent != this)
            {
                SeatingController.instance.SwapStudents(this);
            }

        }
    }

    public void OnMouseRelease()
    {
        
    }
}
