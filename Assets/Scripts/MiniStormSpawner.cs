using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniStormSpawner : MonoBehaviour
{
    [SerializeField] Transform borderStormTransform;
    [SerializeField] Transform borderStormMaskTransform;
    [SerializeField] GameObject stormPrefab;
    [SerializeField] List<Sprite> StormSpritesList = new List<Sprite>();

    [SerializeField] GameObject player;
    [SerializeField] bool TriggerStorms = false;

    Transform playerTransform;
    float stormSpawnMargin = 5f;

    private void Start()
    {
        playerTransform = player.transform;
    }

    private void Update()
    {
        if (TriggerStorms)
            StartStorms();
    }

    public void StartStorms()
    {
        if (borderStormTransform.localScale.x <= 0.01f)
        {
            return;
        }
        StartCoroutine(GenerateStorm());
        TriggerStorms = false;
    }

    IEnumerator GenerateStorm()
    {
        while(true)
        {
            Vector3 location = GenerateLocation();
            GameObject newObject = Instantiate(stormPrefab, location, Quaternion.identity);
            newObject.GetComponent<MiniStorm>().Player = player;
            SpriteRenderer newObjectSprite = newObject.GetComponent<SpriteRenderer>();

            int x = Random.Range(0, 2);
            int randomSprite = Random.Range(0, StormSpritesList.Count);

            newObjectSprite.flipX = x == 0 ? false : true;
            newObjectSprite.sprite = StormSpritesList[randomSprite];

            yield return new WaitForSeconds(5);
        }
    }

    Vector3 GenerateLocation()
    {
        float xRange = (borderStormTransform.localScale.x / 2 * borderStormMaskTransform.localScale.x) - 0.5f + stormSpawnMargin;
        float yRange = (borderStormTransform.localScale.y / 2 * borderStormMaskTransform.localScale.y) - 0.5f + stormSpawnMargin;
        float zRange = (borderStormTransform.localScale.z / 2 * borderStormMaskTransform.localScale.z) - 0.5f + stormSpawnMargin;

        xRange = Random.Range(-xRange, xRange) + borderStormTransform.position.x;
        yRange = Random.Range(-yRange, yRange) + borderStormTransform.position.y;
        zRange = Random.Range(-zRange, zRange) + borderStormTransform.position.z;

        if (Mathf.Abs(playerTransform.position.x - xRange) < stormSpawnMargin && Mathf.Abs(playerTransform.position.y - yRange) < stormSpawnMargin && Mathf.Abs(playerTransform.position.z - zRange) < stormSpawnMargin)
            return new Vector3(xRange + stormSpawnMargin, yRange + stormSpawnMargin, zRange + stormSpawnMargin);

        return new Vector3(xRange, yRange, zRange);
    }

    
}
