using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Transform))]
public class RadialSliderControlledRotation : MonoBehaviour
{
    public Image radialSliderImage;
    public float speed = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, radialSliderImage.fillAmount * 360f), Time.deltaTime * speed);
    }
}
