using System.Collections;
using TMPro;
using UnityEngine;

public class StudentObject : MonoBehaviour, IInteractable
{
    public enum State{Idle, Talking};
    public State state;
    public bool idle => state == State.Idle;
    public bool talking => state == State.Talking;
    [System.NonSerialized]
    public Student data;

    public SpriteRenderer bodyRenderer, faceRenderer, hatRenderer;
    public TextMeshProUGUI nameTag;
    private float talkTimer = 0;

    public Seat seat;

    private Animator animator;

    const float REGULAR_CHANCE = 20;
    const float FRIEND_CHANCE = 50;

    void Awake()
    {
        animator = GetComponent<Animator>();
        talkTimer = 0;
    }
    void Update()
    {
        seat = transform.parent.GetComponent<Seat>();
        if(GameController.instance.viewingStudents)
        {
            if(state == State.Idle)
            {
                if(talkTimer > 0)
                {
                    talkTimer -= Time.deltaTime;
                }
                else
                {
                    foreach (Seat seat in seat.neighboringSeats)
                    {
                        AttemptTalk(seat.studentObject);
                    }
                    talkTimer = data.shyness;
                }
            }
            else
            {
                talkTimer = data.shyness;
            }
        }

        if(transform.parent != null)
        {
            transform.position = Vector3.Lerp(transform.position, transform.parent.position, 5 * Time.deltaTime);
        }

        Animation();
    }

    public void StartTalking()
    {
        state = State.Talking;
        CancelInvoke(nameof(SetOff));
        Invoke(nameof(SetOff), 2);
    }

    private void SetOff()
    {
        state = State.Idle;
    }

    public void AttemptTalk(StudentObject studentObject)
    {
        if(Friends(studentObject))
        {
            if(Success(FRIEND_CHANCE))
            {
                studentObject.StartTalking();
                StartTalking();
            }
        }
        else
        {
            if(Success(REGULAR_CHANCE))
            {
                studentObject.StartTalking();
                StartTalking();
            }
        }
    }

    public bool Success(float chance)
    {
        float result = Random.Range(0f, 100f);
        return result < chance;
    }

    private bool Friends(StudentObject studentObject)
    {
        return data.friends.Contains(studentObject.data);
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

    private void Animation()
    {
        animator.SetBool("Idle", idle);
        animator.SetBool("Talking", talking);
    }
}
