using UnityEngine;

public class OffsetAnimation : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Offset", Random.Range(0f,1f));
    }
}
