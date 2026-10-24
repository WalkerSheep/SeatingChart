using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Student
{
    public string name = "";
    public Sprite body, face;
    public Color bodyColor = Color.white;
    public float shyness = 0;
    [System.NonSerialized]
    public List<Student> friends = new List<Student>();

    public Student(string name, Sprite body, Sprite face, Color bodyColor, float shyness)
    {
        this.name = name;
        this.body = body;
        this.face = face;
        this.bodyColor = bodyColor;
        this.shyness = shyness;
    }

    public void AddFriend(Student friend)
    {
        friends.Add(friend);
    }
}

