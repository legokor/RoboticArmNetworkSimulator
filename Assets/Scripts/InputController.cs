using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public Camera primaryCamera;
    public Camera secondaryCamera;
    public IKSolver ikSolver;
    public Slider horizontalRodSlider;
    public Slider verticalMovingElementSlider;
    public RadialSlider radialSlider;
    public GripButton gripButton;
    public float linearSliderSpeed;
    public float radialSliderSpeed;

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            DecreaseHorizontalExtension(linearSliderSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            IncreaseHorizontalExtension(linearSliderSpeed);
        }
        if (Input.GetKey(KeyCode.W))
        {
            DecreaseVerticalElevation(linearSliderSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            IncreaseVerticalElevation(linearSliderSpeed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            RotateAnticlockwise(radialSliderSpeed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            RotateClockwise(radialSliderSpeed);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (secondaryCamera.pixelRect.Contains(Input.mousePosition))
            {
                SwitchCameras();
            }
            else if (Input.mousePosition.x < Screen.width * 0.75f)
            {
                Grip();
            }
        }
    }

    protected void SwitchCameras()
    {
        Camera tmp = primaryCamera;
        primaryCamera = secondaryCamera;
        secondaryCamera = tmp;
        secondaryCamera.rect = primaryCamera.rect;
        primaryCamera.rect = new Rect(0, 0, 1, 1);
        primaryCamera.depth = 0;
        secondaryCamera.depth = 1;
    }

    public void IncreaseHorizontalExtension(float amount)
    {
        horizontalRodSlider.value += amount;
    }

    public void DecreaseHorizontalExtension(float amount)
    {
        horizontalRodSlider.value -= amount;
    }

    public float GetHorizontalExtensionMin()
    {
        return horizontalRodSlider.minValue;
    }

    public float GetHorizontalExtensionMax()
    {
        return horizontalRodSlider.maxValue;
    }

    public float GetHorizontalExtensionRange()
    {
        return horizontalRodSlider.maxValue - horizontalRodSlider.minValue;
    }

    public void SetHorizontalExtension(float value)
    {
        horizontalRodSlider.value = value;
    }

    public void IncreaseVerticalElevation(float amount)
    {
        verticalMovingElementSlider.value -= amount;
    }

    public void DecreaseVerticalElevation(float amount)
    {
        verticalMovingElementSlider.value += amount;
    }

    public float GetVerticalElevationMin()
    {
        return verticalMovingElementSlider.minValue;
    }

    public float GetVerticalElevationMax()
    {
        return verticalMovingElementSlider.maxValue;
    }

    public float GetVerticalElevationRange()
    {
        return verticalMovingElementSlider.maxValue - verticalMovingElementSlider.minValue;
    }

    public void SetVerticalElevation(float value)
    {
        verticalMovingElementSlider.value = value;
    }

    public void RotateClockwise(float angle)
    {
        radialSlider.IncreaseAngle(angle);
    }

    public void RotateAnticlockwise(float angle)
    {
        radialSlider.IncreaseAngle(-angle);
    }

    public void SetRotationAngle(float angle)
    {
        radialSlider.SetAngle(angle);
    }

    public void Grip()
    {
        if (!gripButton.IsPressed())
            gripButton.Press();
    }

    public void ReleaseGrip()
    {
        if (gripButton.IsPressed())
            gripButton.Press();
    }
}
