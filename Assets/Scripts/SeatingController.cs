using UnityEngine;

public class SeatingController : MonoBehaviour
{
    public static SeatingController instance;

    public StudentObject selectedStudent;

    public Animator selection;

    public Transform hover;

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
        if(hover)
        {
            selection.transform.position = hover.transform.position;
        }
        selection.SetBool("Visible", hover);
    }
}
