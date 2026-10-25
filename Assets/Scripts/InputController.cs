using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController instance;

    public Vector2 mousePosition => Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

    public IInteractable interactable;
    private IInteractable clickedInteractable;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Collider2D hit = Physics2D.OverlapPoint(mousePosition);
        IInteractable newInteractable = null;

        if (hit != null)
        {
            newInteractable = hit.GetComponent<IInteractable>();
        }

        // Check if the hovered object changed
        if (newInteractable != interactable)
        {
            if (interactable != null)
            {
                interactable.OnMouseExit();
            }

            interactable = newInteractable;

            if (interactable != null)
            {
                interactable.OnMouseEnter();
            }
        }

        // Mouse clicked
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            clickedInteractable = interactable;

            if (clickedInteractable != null)
            {
                clickedInteractable.OnMouseClick();
            }
        }

        // Mouse released
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (clickedInteractable != null)
            {
                clickedInteractable.OnMouseRelease();
                clickedInteractable = null;
            }
        }
    }
}

public interface IInteractable
{
    void OnMouseEnter();
    void OnMouseExit();
    void OnMouseClick();
    void OnMouseRelease();
}