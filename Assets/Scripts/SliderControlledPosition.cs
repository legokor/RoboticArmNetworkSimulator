using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControlledPosition : MonoBehaviour
{
    public Slider xOffsetSlider;
    public Slider yOffsetSlider;
    public Slider zOffsetSlider;

    public float speed = 50f;

    protected Vector3 startPos;
    protected Vector3 minPos;
    protected Vector3 maxPos;

    void Start()
    {
        startPos = transform.localPosition;
        minPos = startPos;
        maxPos = startPos;
        if (xOffsetSlider != null)
        {
            minPos.x += xOffsetSlider.minValue;
            maxPos.x += xOffsetSlider.maxValue;
        }
        if (yOffsetSlider != null)
        {
            minPos.y += yOffsetSlider.minValue;
            maxPos.y += yOffsetSlider.maxValue;
        }
        if (zOffsetSlider != null)
        {
            minPos.z += zOffsetSlider.minValue;
            maxPos.z += zOffsetSlider.maxValue;
        }
    }

    void Update()
    {
        Vector3 pos = transform.localPosition;
        if (xOffsetSlider != null)
        {
            float a = (startPos.x + xOffsetSlider.value - pos.x) * speed;
            float dv = a * Time.deltaTime;
            float ds = dv * Time.deltaTime;
            pos.x += ds;
            pos.x = Mathf.Min(pos.x, maxPos.x);
            pos.x = Mathf.Max(pos.x, minPos.x);
        }
        if (yOffsetSlider != null)
        {
            float a = (startPos.y + yOffsetSlider.value - pos.y) * speed;
            float dv = a * Time.deltaTime;
            float ds = dv * Time.deltaTime;
            pos.y += ds;
            pos.y = Mathf.Min(pos.y, maxPos.y);
            pos.y = Mathf.Max(pos.y, minPos.y);
        }
        if (zOffsetSlider != null)
        {
            float a = (startPos.z + zOffsetSlider.value - pos.z) * speed;
            float dv = a * Time.deltaTime;
            float ds = dv * Time.deltaTime;
            pos.z += ds;
            pos.z = Mathf.Min(pos.z, maxPos.z);
            pos.z = Mathf.Max(pos.z, minPos.z);
        }
        transform.localPosition = pos;
    }
}
