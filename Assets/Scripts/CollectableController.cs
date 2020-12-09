using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectableController : MonoBehaviour
{
    protected int nCollectedCollectables = 0;

    public TextMeshProUGUI collectableCounterText;
    public GameObject collectablePrefab;
    public float respawnDelayInSeconds;
    public Transform pivot;
    public Transform target;
    public int targetRelocationCounter;

    public static readonly float rMin = 18.5f;
    public static readonly float rMax = 57f;
    public static readonly float heightMin = 18f;
    public static readonly float heightMax = 50f;

    protected Vector3 lookatPoint;

    void Start()
    {
        lookatPoint = pivot.position + new Vector3(-8f, 0f, 0f);
    }
    
    void Update()
    {
        // For debug purposes
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
            foreach (GameObject collectable in collectables)
            {
                Destroy(collectable);
            }
            StartCoroutine(InstantiateNewCollectable());
        }
    }

    public void RegisterCollectedCollectable()
    {
        nCollectedCollectables++;
        collectableCounterText.text = nCollectedCollectables.ToString();

        if (nCollectedCollectables % targetRelocationCounter == 0)
        {
            float angle = Random.Range(0f, 360f);
            float r = Random.Range(rMin, rMax);

            float z = r * Mathf.Cos(angle);
            float x = r * Mathf.Sin(angle);

            float y = target.position.y - pivot.position.y;

            target.position = pivot.position + new Vector3(x, y, z);
        }

        StartCoroutine(InstantiateNewCollectable());
    }

    public IEnumerator InstantiateNewCollectable()
    {
        yield return new WaitForSeconds(respawnDelayInSeconds);

        float angle = Random.Range(0f, 360f);
        float r = Random.Range(rMin, rMax);

        float z = r * Mathf.Cos(angle);
        float x = r * Mathf.Sin(angle);

        float height = Random.Range(heightMin, heightMax);
        float y = height / 2f + Random.Range(5f, 10f);

        GameObject newCollectable = Instantiate(collectablePrefab, pivot.position + new Vector3(x, y, z), Quaternion.identity) as GameObject;
        newCollectable.transform.LookAt(lookatPoint);
        newCollectable.transform.rotation = Quaternion.Euler(0, newCollectable.transform.rotation.eulerAngles.y, 0);

        Vector3 scale = newCollectable.transform.localScale;
        scale.y = height;
        newCollectable.transform.localScale = scale;
        
        int i = (int)Mathf.Round(Random.Range(0.5f, 7.5f));
        Material material = Resources.Load("Materials/Metal Crates/Materials/crate" + i, typeof(Material)) as Material;
        if (material != null)
        {
            newCollectable.GetComponent<Renderer>().material = material;
        }
    }
}
