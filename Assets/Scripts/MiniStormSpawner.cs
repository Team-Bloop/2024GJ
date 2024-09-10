using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniStormSpawner : MonoBehaviour
{
    [SerializeField] Transform BorderStormTransform;
    [SerializeField] GameObject StormPrefab;
    [SerializeField] bool TriggerStorms = false;

    private void Update()
    {
        if (TriggerStorms)
            StartStorms();
    }

    public void StartStorms()
    {
        StartCoroutine(GenerateStorm());
        TriggerStorms = false;
    }

    IEnumerator GenerateStorm()
    {
        while(true)
        {
            Vector3 location = GenerateLocation();
            Instantiate(StormPrefab, location, Quaternion.identity);
            yield return new WaitForSeconds(5);
        }
    }

    Vector3 GenerateLocation()
    {
        float xRange = (BorderStormTransform.localScale.x / 2) - 0.5f;
        float yRange = (BorderStormTransform.localScale.y / 2) - 0.5f;
        float zRange = (BorderStormTransform.localScale.z / 2) - 0.5f;

        xRange = Random.Range(-xRange, xRange);
        yRange = Random.Range(-yRange, yRange);
        zRange = Random.Range(-zRange, zRange);

        return new Vector3(xRange, yRange, zRange);
    }
}
