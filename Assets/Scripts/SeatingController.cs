using UnityEngine;

public class SeatingController : MonoBehaviour
{
    public static SeatingController instance;

    public StudentObject selectedStudent;

    public Animator selection;

    public Transform hover;

    public bool snap = true;

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
            if(snap)
            {
                selection.transform.position = hover.transform.position; 
            }
            else
            {
                selection.transform.position = Vector3.Lerp(selection.transform.position, hover.transform.position, 10 * Time.deltaTime); 
            }    
        }
        selection.SetBool("Visible", hover);
    }
}
