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

    const float REGULAR_CHANCE = 10;
    const float FRIEND_CHANCE = 75;

    void Awake()
    {
        animator = GetComponent<Animator>();
        talkTimer = 0;
    }

    public void UpdateSeat()
    {
        seat = transform.parent.GetComponent<Seat>();
    }

    void Update()
    {
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

    public void Squish()
    {
        animator.SetTrigger("Squish");
    }

    public void StartTalking()
    {
        state = State.Talking;
        CancelInvoke(nameof(SetOff));
        Invoke(nameof(SetOff), 2.5f);
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
                GameController.instance.SpawnSpeechBubble(this, studentObject);
            }
        }
        else
        {
            if(Success(REGULAR_CHANCE))
            {
                studentObject.StartTalking();
                StartTalking();
                GameController.instance.SpawnSpeechBubble(this, studentObject);
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
        if(GameController.instance.changingChart)
        {
            if(!SeatingController.instance.hover)
            {
                SeatingController.instance.hover = transform;
            }
        }
    }

    public void OnMouseExit()
    {
        if(GameController.instance.changingChart)
        {
            if(SeatingController.instance.hover == transform)
            {
                SeatingController.instance.hover = null;
            }
        }
    }

    public void OnMouseClick()
    {
        Debug.Log(data.name);
        Click();
    }

    public void Click()
    {
        if(GameController.instance.changingChart)
        {
            Squish();
            if(SeatingController.instance.selectedStudent == null)
            {
                SeatingController.instance.line.SetPosition(1, transform.position);
                SeatingController.instance.selectedStudent = this;
            }
            else if(SeatingController.instance.selectedStudent == this)
            {
                SeatingController.instance.selectedStudent = null;
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
