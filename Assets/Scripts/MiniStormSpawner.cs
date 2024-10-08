using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiniStormSpawner : MonoBehaviour
{
    Grid grid;
    Tilemap worldBorder;

    [SerializeField]
    GameObject stormPrefab;
    /*[SerializeField]
    List<Sprite> StormSpritesList = new List<Sprite>();*/

    GameObject player;

    Transform playerTransform;
    PlayerManager playerManager;

    [SerializeField] 
    float spawnRate;
    [SerializeField] 
    float spawnRatio;
    [SerializeField] 
    float stormSizeRatio;
    [SerializeField]
    float maxStormSize = 30;
    [SerializeField]
    int maxStormQuantity = 30;

    public static int StormCount = 0;

    bool triggerStorms = true;
    int[] posNeg = { -1, 1 };

    private void Start()
    {
        player = GameObject.Find("Player");
        playerTransform = player.transform;
        playerManager = player.GetComponent<PlayerManager>();

        grid = GameObject.Find("Grid").GetComponent<Grid>();
        worldBorder = GameObject.Find("WorldBorder").GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (triggerStorms)
            StartStorms();
    }

    public void StartStorms()
    {
        StartCoroutine(GenerateStorm());
        triggerStorms = false;
    }

    IEnumerator GenerateStorm()
    {
        while(true)
        {
            int quantity = (int) Mathf.RoundToInt(spawnRatio * playerManager.GetCurrentLevel());
            for (int i = 0; i < spawnRatio * playerManager.GetCurrentLevel(); i++)
            {
                if (StormCount >= maxStormQuantity)
                    break;
                Vector3 newScale = GenerateStormScale();
                Vector3 location = GenerateLocation(newScale.x);
                GameObject newObject = Instantiate(stormPrefab, location, Quaternion.identity);
                newObject.transform.localScale = newScale;
                StormCount++;
                //SpriteRenderer newObjectSprite = newObject.GetComponent<SpriteRenderer>();

                /*int x = Random.Range(0, 2);
                int randomSprite = Random.Range(0, StormSpritesList.Count);

                newObjectSprite.flipX = x == 0 ? false : true;
                newObjectSprite.sprite = StormSpritesList[randomSprite];*/
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }

    Vector3 GenerateStormScale()
    {
        float maxRnd = stormSizeRatio * playerManager.GetCurrentLevel();
        if (maxRnd > 30)
        {
            maxRnd = 30;
        }
        float randomScale = Random.Range(1, maxRnd);
        return Vector3.one * randomScale;
    }

    Vector3 GenerateLocation(float distance)
    {
        float xRangeMin = (worldBorder.cellBounds.min.x * grid.cellSize.x) + distance;
        float xRangeMax = (worldBorder.cellBounds.max.x * grid.cellSize.x) + distance;

        float yRangeMin = (worldBorder.cellBounds.min.y * grid.cellSize.x) + distance;
        float yRangeMax = (worldBorder.cellBounds.max.y * grid.cellSize.x) + distance;

        float xRange = Random.Range(xRangeMin, xRangeMax);
        float yRange = Random.Range(yRangeMin, yRangeMax);

        if (Mathf.Abs(playerTransform.position.x - xRange) < distance && Mathf.Abs(playerTransform.position.y - yRange) < distance)
        {
            int posNegX;
            int posNegY;
            if (playerTransform.position.x >= xRange)
                posNegX = 0;
            else
                posNegX = 1;

            if (playerTransform.position.y >= yRange)
                posNegY = 0;
            else
                posNegY = 1;

            return new Vector3(xRange + (distance * posNeg[posNegX]), yRange + (distance * posNeg[posNegY]));
        }

        return new Vector3(xRange, yRange, 0);
    }
}
