using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GripController : MonoBehaviour
{
    public GripButton gripButton;
    public GameObject closerConnector;
    public GameObject closerFinger;
    public GameObject fartherConnector;
    public GameObject fartherFinger;
    public float speed = 250.0f;

    private Fingertip closerFingertip;
    private Fingertip fartherFingertip;

    private float maxElevation = 20.0f;
    private float connectorMaxY = 25.0f;
    private float connectorMaxZ = 12.0f;
    private float connectorMaxRotation = 8.0f;
    private float fingerMaxY = 40.0f;
    private float fingerMaxZ = 25.0f;
    private float fingerMaxRotation = 25.0f;

    void Start()
    {
        closerFingertip = closerFinger.GetComponentInChildren<Fingertip>();
        fartherFingertip = fartherFinger.GetComponentInChildren<Fingertip>();
    }

    void Update()
    {
        float elevation = transform.localPosition.y;

        float a = 0f;

        if (!closerFingertip.IsCollidingWithCollectable() || !fartherFingertip.IsCollidingWithCollectable())
        {
            if (gripButton.IsPressed())
            {
                a = (maxElevation - transform.localPosition.y) * speed;
            }
            else
            {
                a = -transform.localPosition.y * speed;
            }
        }
        
        float dv = a * Time.deltaTime;
        elevation += dv * Time.deltaTime;
        elevation = Mathf.Min(elevation, maxElevation);
        elevation = Mathf.Max(elevation, 0);

        Vector3 newLocalPos = transform.localPosition;
        newLocalPos.y = elevation;
        transform.localPosition = newLocalPos;

        float t = elevation / maxElevation;
        closerConnector.transform.localPosition = new Vector3(0, t * connectorMaxY, t * -connectorMaxZ);
        closerConnector.transform.localRotation = Quaternion.Euler(t * connectorMaxRotation, 0, 0);
        fartherConnector.transform.localPosition = new Vector3(0, t * connectorMaxY, t * connectorMaxZ);
        fartherConnector.transform.localRotation = Quaternion.Euler(t * -connectorMaxRotation, 0, 0);
        closerFinger.transform.localPosition = new Vector3(0, t * fingerMaxY, t * -fingerMaxZ);
        closerFinger.transform.localRotation = Quaternion.Euler(t * fingerMaxRotation, 0, 0);
        fartherFinger.transform.localPosition = new Vector3(0, t * fingerMaxY, t * fingerMaxZ);
        fartherFinger.transform.localRotation = Quaternion.Euler(t * -fingerMaxRotation, 0, 0);
    }
}
