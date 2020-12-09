using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class GripButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Sprite pressedStateSprite;
    public Sprite releasedStateSprite;

    private Image image;
    private bool isPressed;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Press();
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public bool IsPressed()
    {
        return isPressed;
    }

    public void Press()
    {
        if (isPressed)
        {
            isPressed = false;
            image.sprite = releasedStateSprite;
        }
        else
        {
            isPressed = true;
            image.sprite = pressedStateSprite;
        }
    }
}
