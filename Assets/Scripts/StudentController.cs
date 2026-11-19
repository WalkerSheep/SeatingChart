using System.Collections.Generic;
using UnityEngine;

public class StudentController : MonoBehaviour
{
    public static StudentController instance;

    public TextAsset studentFirstNames, studentLastNames;
    public Sprite[] bodySprites, faceSprites, hats;
    public Color[] bodyColors = {Color.white};

    public GameObject studentPrefab;

    public Transform[] seats;

    public static List<Student> students = new List<Student>();

    void Awake()
    {
        instance = this;

        students.Clear();
    }

    public void GenerateStudents()
    {
        students.Clear();
        for (int i = 0; i < seats.Length; i++)
        {
            Sprite hat = null;
            if(Random.Range(0, 2) == 1)
            {
                hat = RandomSprite(hats);
            }
            Student student = new Student(RandomName(studentFirstNames) + " " + RandomName(studentLastNames), RandomSprite(bodySprites), RandomSprite(faceSprites), hat, RandomColor(bodyColors), Random.Range(0.5f, 5f));
            StudentObject studentObject = Instantiate(studentPrefab).GetComponent<StudentObject>();
            studentObject.name = student.name + " (Student)";
            studentObject.Initialize(student);
            studentObject.transform.SetParent(seats[i]);
            studentObject.transform.localPosition = new Vector3(0,0,0);
            students.Add(student);
        }

        RandomizeStudentFriends(students);
    }

    private void RandomizeStudentFriends(List<Student> students)
    {
        foreach (Student student in students)
        {
            int friendCount = Random.Range(1, 4);

            for (int i = 0; i < friendCount; i++)
            {
                Student randomStudent = students[Random.Range(0, students.Count)];

                student.AddFriend(randomStudent);
            }
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
