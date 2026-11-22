using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public StudentObject student1, student2;
    public LineRenderer line1, line2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        student1 = null;
        student2 = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(student1 && student2)
        {
            line1.SetPosition(0, transform.position);
            line2.SetPosition(0, transform.position);

            line1.SetPosition(1, student1.transform.position);
            line2.SetPosition(1, student2.transform.position);

            if(!student1.talking && !student2.talking)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
