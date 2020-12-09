using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Based on: http://unitycoder.com/blog/2015/05/17/radial-slider-ui-test/

[RequireComponent(typeof(Image))]
public class RadialSlider: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image image;
    private Text text;
	bool isPointerDown = false;

    void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
    }

	// Called when the pointer enters our GUI component.
	// Start tracking the mouse
	public void OnPointerEnter( PointerEventData eventData )
	{
		StartCoroutine( "TrackPointer" );            
	}
	
	// Called when the pointer exits our GUI component.
	// Stop tracking the mouse
	public void OnPointerExit( PointerEventData eventData )
	{
		StopCoroutine( "TrackPointer" );
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		isPointerDown= true;
		//Debug.Log("mousedown");
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isPointerDown= false;
		//Debug.Log("mousedown");
	}

	// mainloop
	IEnumerator TrackPointer()
	{
        var raycaster = GetComponentInParent<GraphicRaycaster>();

        if (raycaster != null)
		{
			while (Application.isPlaying)
			{                    

				// TODO: if mousebutton down
				if (isPointerDown)
				{
					Vector2 localPos; // Mouse position  
					RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, raycaster.eventCamera, out localPos);
						
					// local pos is the mouse position.
					float angle = (Mathf.Atan2(-localPos.y, localPos.x)*180f/Mathf.PI+180f)/360f;

                    //Debug.Log(localPos + ": " + angle);	

                    SetAngle(angle);
				}

				yield return 0;
			}        
		}
		else
			UnityEngine.Debug.LogWarning( "Could not find GraphicRaycaster and/or StandaloneInputModule" );        
	}

    public void SetAngle(float angle)
    {
        while (angle >= 1.0f)
        {
            angle -= 1.0f;
        }
        while (angle < 0.0f)
        {
            angle += 1.0f;
        }

        image.fillAmount = angle;
        //image.color = Color.Lerp(Color.green, Color.red, angle);
        text.text = ((int)(angle * 360f)).ToString();
    }

    public void IncreaseAngle(float dAngle)
    {
        SetAngle(image.fillAmount + dAngle);
    }

}
