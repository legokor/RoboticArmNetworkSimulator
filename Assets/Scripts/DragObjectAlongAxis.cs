using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class DragObjectAlongAxis : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public float horizontalOffsetMin;
    public float horizontalOffsetMax;
    public float verticalOffsetMin;
    public float verticalOffsetMax;
    public float depthOffsetMin;
    public float depthOffsetMax;

    private Vector3 posAtStart;
    private Vector3 posAtBeginDrag;
    private Vector3 mousePosAtBeginDrag;

    void Start()
    {
        if (Camera.main.GetComponent<PhysicsRaycaster>() == null)
            Debug.Log("Camera doesn't have a physics raycaster.");

        posAtStart = transform.localPosition;
    }

    private Vector3 getMousePositionInWorldPlace()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 planeOrigin = transform.localPosition;
        Vector3 planeNormal = -transform.right;
        float t = Vector3.Dot(planeOrigin - ray.origin, planeNormal) / Vector3.Dot(ray.direction, planeNormal);
        return ray.origin + ray.direction * t;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = getMousePositionInWorldPlace();
        Vector3 offsetFromBeginDrag = mousePos - mousePosAtBeginDrag;
        Vector3 pos = posAtBeginDrag + offsetFromBeginDrag;
        Vector3 offsetFromStart = pos - posAtStart;

        float horizontalOffsetFromStart = Vector3.Dot(offsetFromStart, transform.up);
        Debug.Log(horizontalOffsetFromStart);
        if (horizontalOffsetFromStart > horizontalOffsetMax)
        {
            pos -= (horizontalOffsetFromStart - horizontalOffsetMax) * transform.up;
        }
        else if (horizontalOffsetFromStart < horizontalOffsetMin)
        {
            pos += (horizontalOffsetMin - horizontalOffsetFromStart) * transform.up;
        }

        float verticalOffsetFromStart = Vector3.Dot(offsetFromStart, transform.forward);
        if (verticalOffsetFromStart > verticalOffsetMax)
        {
            pos -= (verticalOffsetFromStart - verticalOffsetMax) * transform.forward;
        }
        else if (verticalOffsetFromStart < verticalOffsetMin)
        {
            pos += (verticalOffsetMin - verticalOffsetFromStart) * transform.forward;
        }

        float depthOffsetFromStart = Vector3.Dot(offsetFromStart, transform.right);
        if (depthOffsetFromStart > depthOffsetMax)
        {
            pos -= (depthOffsetFromStart - depthOffsetMax) * transform.right;
        }
        else if (depthOffsetFromStart < depthOffsetMin)
        {
            pos += (depthOffsetMin - depthOffsetFromStart) * transform.right;
        }

        transform.localPosition = pos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        posAtBeginDrag = transform.localPosition;
        mousePosAtBeginDrag = getMousePositionInWorldPlace();
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}