using UnityEngine;
using UnityEngine.EventSystems; // Required for touch and UI events

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Will be true while thumb is on the button,false when lifted
    [HideInInspector]
    public bool isPressed = false;

    // Triggered when the screen is touched
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

}

