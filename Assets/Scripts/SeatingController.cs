
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class SeatLane
{
    public Seat[] seats;
}

public class SeatingController : MonoBehaviour
{
    public static SeatingController instance;

    public StudentObject selectedStudent;

    public Animator selection;
    public Animator marker;
    public LineRenderer line;
    public Transform hover;

    public SeatLane[] lanes;

    public int currentLane = 0;
    public int currentHover = 0;

    public bool controllerConnected => Gamepad.current != null;
    public bool snap => !controllerConnected;

    public float stickDeadzone = 0.1f;
    public float inputSendDelay = 0.15f;

    private bool stickMoved = false;
    private bool stickSent = false;
    private float stickTimer = 0;

    private int horizontalInput = 0;
    private int verticalInput = 0;

    private int startingLane = 0;
    private int startingHover = 0;

    private int sentHorizontal = 0;
    private int sentVertical = 0;

    void Awake()
    {
        instance = this;
    }

    public void SwapStudents(StudentObject swap)
    {
        Transform temp = selectedStudent.transform.parent;
        selectedStudent.transform.parent = swap.transform.parent;
        swap.transform.parent = temp;

        StudentController.instance.SeatingChanged();

        selectedStudent = null;
    }

    void Update()
    {
        line.enabled = selectedStudent;
        if(selectedStudent)
        {
            line.SetPosition(0, selectedStudent.transform.position);
            if(hover)
            {
                line.SetPosition(1, Vector3.Lerp(line.GetPosition(1), hover.transform.position, 50 * Time.deltaTime));
            }
            else
            {
                line.SetPosition(1, Vector3.Lerp(line.GetPosition(1), InputController.instance.mousePosition, 50 * Time.deltaTime));
            }
        }
        marker.SetBool("Visible", selectedStudent);
        if(selectedStudent)
        {
            marker.transform.position = selectedStudent.transform.position + new Vector3(0,1f,0);
        }
        if(controllerConnected)
        {
            if(GameController.instance.changingChart && ValidSeat())
            {
                ControllerMovement();

                Seat seat = lanes[currentLane].seats[currentHover];

                if(Gamepad.current.buttonSouth.wasPressedThisFrame)
                {
                    if(seat.studentObject != null)
                    {
                        seat.studentObject.Click();
                    }
                }

                if(seat.studentObject != null)
                {
                    hover = seat.studentObject.transform;
                }
                else
                {
                    hover = seat.transform;
                }
            }
            else
            {
                hover = null;
                ResetStick();
            }
        }

        if(hover)
        {
            if(snap)
            {
                selection.transform.position = hover.position;
            }
            else
            {
                selection.transform.position = Vector3.Lerp(selection.transform.position, hover.position, 25 * Time.deltaTime);
            }
        }

        selection.SetBool("Visible", hover != null);
    }

    void ControllerMovement()
    {
        Vector2 stick = Gamepad.current.leftStick.ReadValue();

        if(stick.magnitude > stickDeadzone)
        {
            if(!stickMoved)
            {
                stickMoved = true;
                stickTimer = 0;

                startingLane = currentLane;
                startingHover = currentHover;
            }

            if(stick.x > 0.35f)
            {
                horizontalInput = 1;
            }
            else if(stick.x < -0.35f)
            {
                horizontalInput = -1;
            }

            if(stick.y > 0.35f)
            {
                verticalInput = 1;
            }
            else if(stick.y < -0.35f)
            {
                verticalInput = -1;
            }

            if(!stickSent)
            {
                stickTimer += Time.deltaTime;

                if(stickTimer >= inputSendDelay)
                {
                    SendStickInput();
                }
            }
            else if(horizontalInput != sentHorizontal || verticalInput != sentVertical)
            {
                SendStickInput();
            }
        }
        else if(stickMoved)
        {
            if(!stickSent)
            {
                SendStickInput();
            }

            ResetStick();
        }
    }

    void SendStickInput()
    {
        currentLane = startingLane;
        currentHover = startingHover;

        if(horizontalInput != 0)
        {
            MoveHorizontal(horizontalInput, verticalInput);
        }
        else if(verticalInput != 0)
        {
            MoveVertical(-verticalInput);
        }

        sentHorizontal = horizontalInput;
        sentVertical = verticalInput;

        stickSent = true;
    }

    void ResetStick()
    {
        stickMoved = false;
        stickSent = false;
        stickTimer = 0;

        horizontalInput = 0;
        verticalInput = 0;

        sentHorizontal = 0;
        sentVertical = 0;
    }

    void MoveVertical(int direction)
    {
        Seat[] seats = lanes[currentLane].seats;

        for(int i = 0; i < seats.Length; i++)
        {
            currentHover += direction;

            if(currentHover >= seats.Length)
            {
                currentHover = 0;
            }
            else if(currentHover < 0)
            {
                currentHover = seats.Length - 1;
            }

            if(seats[currentHover] != null)
            {
                break;
            }
        }
    }

    void MoveHorizontal(int direction, int vertical)
    {
        if(lanes.Length < 2)
        {
            return;
        }

        float currentY = lanes[currentLane].seats[currentHover].transform.position.y;

        for(int i = 1; i < lanes.Length; i++)
        {
            int nextLane = (currentLane + direction * i + lanes.Length) % lanes.Length;

            if(lanes[nextLane] == null || lanes[nextLane].seats == null)
            {
                continue;
            }

            Seat[] seats = lanes[nextLane].seats;

            float closest = float.MaxValue;
            int closestIndex = -1;

            float fallback = float.MaxValue;
            int fallbackIndex = -1;

            for(int j = 0; j < seats.Length; j++)
            {
                if(seats[j] != null)
                {
                    float y = seats[j].transform.position.y;
                    float distance = Mathf.Abs(currentY - y);

                    if(distance < fallback)
                    {
                        fallback = distance;
                        fallbackIndex = j;
                    }

                    bool valid = true;

                    if(vertical > 0 && y <= currentY)
                    {
                        valid = false;
                    }
                    else if(vertical < 0 && y >= currentY)
                    {
                        valid = false;
                    }

                    if(valid && distance < closest)
                    {
                        closest = distance;
                        closestIndex = j;
                    }
                }
            }

            if(closestIndex == -1)
            {
                closestIndex = fallbackIndex;
            }

            if(closestIndex != -1)
            {
                currentLane = nextLane;
                currentHover = closestIndex;
                break;
            }
        }
    }

    bool ValidSeat()
    {
        if(lanes == null || lanes.Length == 0)
        {
            return false;
        }

        if(currentLane >= 0 && currentLane < lanes.Length)
        {
            if(lanes[currentLane] != null && lanes[currentLane].seats != null)
            {
                Seat[] seats = lanes[currentLane].seats;

                if(currentHover >= 0 && currentHover < seats.Length)
                {
                    if(seats[currentHover] != null)
                    {
                        return true;
                    }
                }
            }
        }

        for(int i = 0; i < lanes.Length; i++)
        {
            if(lanes[i] != null && lanes[i].seats != null)
            {
                for(int j = 0; j < lanes[i].seats.Length; j++)
                {
                    if(lanes[i].seats[j] != null)
                    {
                        currentLane = i;
                        currentHover = j;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if(device is Gamepad)
        {
            if(change == InputDeviceChange.Added)
            {
                currentLane = 0;
                currentHover = 0;
                ResetStick();
            }

            if(change == InputDeviceChange.Removed)
            {
                hover = null;
                ResetStick();
            }
        }
    }
}
