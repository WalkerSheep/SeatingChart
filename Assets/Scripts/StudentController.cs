using System.Collections.Generic;
using UnityEngine;

public class StudentController : MonoBehaviour
{
    public static StudentController instance;

    public TextAsset studentFirstNames, studentLastNames;
    public Sprite[] bodySprites, faceSprites, hats;
    public Color[] bodyColors = {Color.white};

    public GameObject studentPrefab;

    public Seat[] seats;

    public static List<Student> students = new List<Student>();

    public static Dictionary<int, Student> studentSeats = new Dictionary<int, Student>();

    void Awake()
    {
        instance = this;
    }

    public void PlaceStudents()
    {
        for (int i = 0; i < seats.Length; i++)
        {
            StudentObject studentObject = Instantiate(studentPrefab).GetComponent<StudentObject>();
            studentObject.name = studentSeats[i].name + " (Student)";
            studentObject.Initialize(studentSeats[i]);
            studentObject.transform.SetParent(seats[i].transform);
            studentObject.transform.localPosition = new Vector3(0,0,0);
        }

        SeatingChanged();
    }

    public void GenerateStudents()
    {
        students.Clear();
        studentSeats.Clear();
        for (int i = 0; i < seats.Length; i++)
        {
            Sprite hat = null;
            if(Random.Range(0, 2) == 1)
            {
                hat = RandomSprite(hats);
            }
            Student student = new Student(RandomName(studentFirstNames) + " " + RandomName(studentLastNames), RandomSprite(bodySprites), RandomSprite(faceSprites), hat, RandomColor(bodyColors), Random.Range(2.5f, 10f));
            students.Add(student);
            studentSeats.Add(i, student);
        }

        RandomizeStudentFriends(students);
    }

    void UpdateStudentSeats()
    {
        studentSeats.Clear();
        for (int i = 0; i < seats.Length; i++)
        {
            if(seats[i].studentObject)
            {
                studentSeats.Add(i, seats[i].studentObject.data);
            }
        }
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

    public void SeatingChanged()
    {
        foreach (Seat seat in seats)
        {
            seat.UpdateStudent();
        }
        UpdateStudentSeats();
    }
}
