using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(TriangleExplosion))]
public class Collectable : MonoBehaviour
{
    protected CollectableController collectableController;
    protected Collider fingertip1;
    protected Collider fingertip2;
    protected Collider target;
    protected Transform pivot;
    protected TextMeshProUGUI popupText;

    protected Collider thisCollider;
    protected Rigidbody thisRigidbody;
    protected TriangleExplosion thisExplosion;

    protected bool isCollidingWithFingertip1 = false;
    protected bool isCollidingWithFingertip2 = false;
    protected bool isTriggeredByFingertip1 = false;
    protected bool isTriggeredByFingertip2 = false;

    protected Vector3 fingertip1PosOnCollision;
    protected Vector3 fingertip2PosOnCollision;
    protected Vector3 pickablePosOnCollision;

    protected float startPosY;

    protected Vector3 baseline;
    protected Vector3 lookatOffset;

    protected bool selfdestroyInitiated = false;

    void Start()
    {
        collectableController = FindObjectsOfType(typeof(CollectableController))[0] as CollectableController;
        fingertip1 = GameObject.Find("Closer Finger/Tip").GetComponent<Collider>();
        fingertip2 = GameObject.Find("Farther Finger/Tip").GetComponent<Collider>();
        target = GameObject.Find("Target").GetComponent<Collider>();
        popupText = GetComponentInChildren<TextMeshProUGUI>();
        //popupText.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, transform.up) * Quaternion.Euler(180f, 0, 0);
        popupText.enabled = false;
        thisCollider = GetComponent<Collider>();
        thisRigidbody = GetComponent<Rigidbody>();
        thisExplosion = GetComponent<TriangleExplosion>();
        startPosY = transform.position.y;
        pivot = GameObject.Find("Ground").transform;
        baseline = new Vector3(pivot.position.x - 8f, pivot.position.y, pivot.position.z - 43.5f);
        lookatOffset = new Vector3(-8f, 0f, 0f);
    }

    void Update()
    {
        if ((isCollidingWithFingertip1 || isTriggeredByFingertip1) && (isCollidingWithFingertip2 || isTriggeredByFingertip2))
        {
            Vector3 newPos = pickablePosOnCollision + ((fingertip1.bounds.center - fingertip1PosOnCollision) + (fingertip2.bounds.center - fingertip2PosOnCollision)) / 2.0f;
            newPos.y = Mathf.Max(newPos.y, startPosY);
            transform.position = newPos;

            Vector3 collectableOffset = transform.position - pivot.position;
            collectableOffset.y = 0;
            float angle = Vector3.SignedAngle(baseline, collectableOffset, Vector3.up);
            Vector3 lookatPoint = Quaternion.AngleAxis(angle, Vector3.up) * lookatOffset;
            transform.LookAt(lookatPoint);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            thisCollider.isTrigger = true;
            thisRigidbody.isKinematic = true;
        }
        else
        {
            thisCollider.isTrigger = false;
            thisRigidbody.isKinematic = false;
        }

        if (!selfdestroyInitiated && shouldDestroySelf())
        {
            StartCoroutine(WaitAndDestroy());
        }
    }

    protected IEnumerator WaitAndDestroy()
    {
        selfdestroyInitiated = true;
        yield return new WaitForSeconds(10);
        if (shouldDestroySelf())
        {
            Destroy(gameObject);
            collectableController.StartCoroutine(collectableController.InstantiateNewCollectable());
        }
    }

    protected bool shouldDestroySelf()
    {
        return (transform.position - pivot.position).sqrMagnitude > (CollectableController.rMax * CollectableController.rMax)
            || Vector3.Angle(transform.up, pivot.up) > 85f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == fingertip1)
        {
            isCollidingWithFingertip1 = true;
            fingertip1PosOnCollision = fingertip1.bounds.center;
            pickablePosOnCollision = transform.position;
        }
        else if (collision.collider == fingertip2)
        {
            isCollidingWithFingertip2 = true;
            fingertip2PosOnCollision = fingertip2.bounds.center;
            pickablePosOnCollision = transform.position;
        }
        else if (collision.collider == target)
        {
            StartCoroutine(thisExplosion.SplitMesh(true));
            popupText.enabled = true;
            collectableController.RegisterCollectedCollectable();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider == fingertip1)
        {
            isCollidingWithFingertip1 = false;
        }
        else if (collision.collider == fingertip2)
        {
            isCollidingWithFingertip2 = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider == fingertip1)
        {
            fingertip1PosOnCollision = fingertip1.bounds.center;
            pickablePosOnCollision = transform.position;
        }
        else if (collision.collider == fingertip2)
        {
            fingertip2PosOnCollision = fingertip2.bounds.center;
            pickablePosOnCollision = transform.position;
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider == fingertip1)
        {
            isTriggeredByFingertip1 = true;
        }
        else if (otherCollider == fingertip2)
        {
            isTriggeredByFingertip2 = true;
        }
        else if (otherCollider == target)
        {
            BeCollected();
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider == fingertip1)
        {
            isTriggeredByFingertip1 = false;
        }
        else if (otherCollider == fingertip2)
        {
            isTriggeredByFingertip2 = false;
        }
        else if (otherCollider == target)
        {
            BeCollected();
        }
    }

    protected void BeCollected()
    {
        StartCoroutine(thisExplosion.SplitMesh(true));
        popupText.enabled = true;
        collectableController.RegisterCollectedCollectable();
    }
}
