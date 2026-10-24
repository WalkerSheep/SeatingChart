using TMPro;
using UnityEngine;

public class StudentObject : MonoBehaviour
{
    [System.NonSerialized]
    public Student data;

    public SpriteRenderer bodyRenderer, faceRenderer;
    public TextMeshProUGUI nameTag;

    public void OnEnable()
    {
        
    }

    public void Initialize(Student data)
    {
        this.data = data;
        
        nameTag.text = data.name;
        bodyRenderer.sprite = data.body;
        faceRenderer.sprite = data.face;
        bodyRenderer.color = data.bodyColor;
    }
}
