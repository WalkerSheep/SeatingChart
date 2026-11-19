using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    public List<Seat> neighboringSeats = new List<Seat>();
    public StudentObject studentObject;

    public void UpdateStudent()
    {
        studentObject = GetComponentInChildren<StudentObject>();
        studentObject.UpdateSeat();
    }
}
