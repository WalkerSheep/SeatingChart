using System.Collections.Generic;
using UnityEngine;

public class StudentController : MonoBehaviour
{
    public static StudentController instance;

    public TextAsset studentFirstNames, studentLastNames;
    public Sprite[] bodySprites, faceSprites;
    public Color[] bodyColors = {Color.white};

    public GameObject studentPrefab;

    public Transform[] seats;

    public List<Student> students = new List<Student>();

    void Awake()
    {
        instance = this;
        
        students.Clear();
    }

    void Start()
    {
        GenerateStudents();
    }

    public void GenerateStudents()
    {
        for (int i = 0; i < seats.Length; i++)
        {
            Student student = new Student(RandomName(studentFirstNames) + " " + RandomName(studentLastNames), RandomSprite(bodySprites), RandomSprite(faceSprites), RandomColor(bodyColors), Random.Range(0.5f, 5f));
            StudentObject studentObject = Instantiate(studentPrefab).GetComponent<StudentObject>();
            studentObject.name = "Student (" + student.name + ")";
            studentObject.Initialize(student);
            studentObject.transform.SetParent(seats[i]);
            studentObject.transform.localPosition = new Vector3(0,0,0);
            students.Add(student);
        }
    }

    public string RandomName(TextAsset textAsset)
    {
        string[] names = textAsset.text.Split('\n');
        return names[Random.Range(0, names.Length)].Trim();
    }

    public Sprite RandomSprite(Sprite[] sprites)
    {
        return sprites[Random.Range(0, sprites.Length)];
    }

    public Color RandomColor(Color[] colors)
    {
        return colors[Random.Range(0, colors.Length)];
    }
}
