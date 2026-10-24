using System.Collections.Generic;
using UnityEngine;

public class StudentController : MonoBehaviour
{
    public TextAsset studentFirstNames, studentLastNames;
    public Sprite[] bodySprites, faceSprites;
    public Color[] bodyColors = {Color.white};

    public GameObject studentPrefab;

    public List<Student> students = new List<Student>();

    void Awake()
    {
        students.Clear();
    }

    void Start()
    {
        GenerateStudents(20);
    }

    public void GenerateStudents(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Student student = new Student(RandomName(studentFirstNames) + " " + RandomName(studentLastNames), RandomSprite(bodySprites), RandomSprite(faceSprites), RandomColor(bodyColors), Random.Range(0.5f, 5f));
            StudentObject studentObject = Instantiate(studentPrefab).GetComponent<StudentObject>();
            studentObject.Initialize(student);
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
