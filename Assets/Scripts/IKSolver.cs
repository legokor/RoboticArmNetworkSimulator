using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSolver : MonoBehaviour {
    public InputController inputController;
    public Transform pivot;
    public Transform target;
    public float delayInSeconds;
    public float elevation;

    private Vector3 baseline;

    void Start()
    {
        baseline = new Vector3(pivot.position.x - 9f, pivot.position.y, pivot.position.z - 43.5f);
    }

    public void Solve()
    {
        StartCoroutine("doSolve");
    }

    private IEnumerator doSolve()
    {
        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
        if (collectables.Length != 1)
        {
            Debug.Log("Error, found " + collectables.Length + " collectables instead of 1");
            yield break;
        }

        GameObject collectable = collectables[0];

        float minHorizontalExtension = inputController.GetHorizontalExtensionMin();
        float maxHorizontalExtension = inputController.GetHorizontalExtensionMax();
        float minVerticalElevation = inputController.GetVerticalElevationMin();
        float maxVerticalElevation = 0; // CollectableController.heightMax is adjusted to y=0 and not to y=max vertical elevation
        float horizontalExtensionRange = maxHorizontalExtension - minHorizontalExtension;
        float verticalElevationRange = maxVerticalElevation - minVerticalElevation;
        float collectableRadiusRange = CollectableController.rMax - CollectableController.rMin;
        float collectableHeightRange = CollectableController.heightMax - CollectableController.heightMin;

        inputController.SetVerticalElevation(minVerticalElevation + (verticalElevationRange / collectableHeightRange) * (collectable.GetComponent<Renderer>().bounds.max.y - CollectableController.heightMin) + elevation);

        yield return new WaitForSeconds(delayInSeconds);

        Vector3 collectableOffset = collectable.transform.position - pivot.position;
        collectableOffset.y = 0;

        float angle = Vector3.SignedAngle(baseline, collectableOffset, Vector3.up);

        if (angle < 0f)
        {
            angle += 360f;
        }

        inputController.SetRotationAngle(angle / 360f);

        yield return new WaitForSeconds(delayInSeconds);
      
        inputController.SetHorizontalExtension(minHorizontalExtension + (horizontalExtensionRange / collectableRadiusRange) * (collectableOffset.magnitude - CollectableController.rMin));

        yield return new WaitForSeconds(delayInSeconds);

        inputController.SetVerticalElevation(minVerticalElevation + (verticalElevationRange / collectableHeightRange) * (collectable.GetComponent<Renderer>().bounds.max.y - CollectableController.heightMin));

        yield return new WaitForSeconds(delayInSeconds);

        inputController.Grip();

        yield return new WaitForSeconds(delayInSeconds);

        inputController.SetVerticalElevation(minVerticalElevation + (verticalElevationRange / collectableHeightRange) * (collectable.GetComponent<Renderer>().bounds.max.y - CollectableController.heightMin) + elevation);

        yield return new WaitForSeconds(delayInSeconds);

        Vector3 targetOffset = target.position - pivot.position;
        targetOffset.y = 0;

        angle = Vector3.SignedAngle(baseline, targetOffset, Vector3.up);

        if (angle < 0f)
        {
            angle += 360f;
        }

        inputController.SetRotationAngle(angle / 360f);

        yield return new WaitForSeconds(delayInSeconds);

        inputController.SetHorizontalExtension(minHorizontalExtension + (horizontalExtensionRange / collectableRadiusRange) * (targetOffset.magnitude - CollectableController.rMin));

        yield return new WaitForSeconds(delayInSeconds);

        inputController.ReleaseGrip();
    }
}
