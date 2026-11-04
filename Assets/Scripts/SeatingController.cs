using UnityEngine;

public class SeatingController : MonoBehaviour
{
    public static SeatingController instance;

    public StudentObject selectedStudent;

    void Awake()
    {
        instance = this;
    }

    public void SwapStudents(StudentObject swap)
    {
        Transform temp = selectedStudent.transform.parent;
        selectedStudent.transform.parent = swap.transform.parent;
        swap.transform.parent = temp;

        selectedStudent = null;
    }
}
