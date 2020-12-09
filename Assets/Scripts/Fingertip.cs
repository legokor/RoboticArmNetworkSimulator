using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Fingertip : MonoBehaviour
{
    private bool isCollidingWithCollectable = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Collectable")
        {
            isCollidingWithCollectable = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Collectable")
        {
            isCollidingWithCollectable = false;
        }
    }

    public bool IsCollidingWithCollectable()
    {
        return isCollidingWithCollectable;
    }
}
