using System;
using UnityEngine;
using GeneralUtility;

public class OrbSpawner : MonoBehaviour
{
    [Header("Boundaries")]
    [SerializeField]
    private Transform[] locations;

    [Header("Spawner")]
    [SerializeField]
    private Orb orbPrefab;

    [SerializeField]
    [Min(0)]
    private int initialOrbSpawn;

    [SerializeField]
    [Min(1)]
    private int maxOrbSpawn;

    [SerializeField]
    [Tooltip("Spawn every `x` seconds")]
    [Min(0.1f)]
    private float spawnRate;

    [SerializeField]
    [Min(1)]
    private int orbsPerSpawn;

    private int orbCount;
    private float currentTime;

    private void Start()
    {
        orbCount = GetComponentsInChildren<Orb>().Length;  
        currentTime = 0f;

        if (locations.Length == 0)
        {
            Utility.Quit();
            throw new ArgumentException("No values provided for Locations");
        }

        foreach (Transform t in locations)
        {
            if (t == null)
            {
                Utility.Quit();
                throw new ArgumentException("An element in Locations is null");
            }

            t.GetComponent<SpriteRenderer>().sortingOrder = -2;
        }

        if (orbCount + initialOrbSpawn > maxOrbSpawn)
        {
            Utility.Quit();
            throw new ArgumentException("Child Orbs in OrbSpawner + InitialOrbSpawn exceeded maxOrbSpawn");
        }

        if (initialOrbSpawn > 0)
        {
            Spawn(initialOrbSpawn); // Spawning will increment orbCount
        }
    }

    private void Reset()
    {
        maxOrbSpawn = 1;
        spawnRate = 0.1f;
        orbsPerSpawn = 1;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= spawnRate && orbCount < maxOrbSpawn)
        {
            if (orbCount + orbsPerSpawn <= maxOrbSpawn)
            {
                Spawn(orbsPerSpawn);
            } else
            {
                Spawn(maxOrbSpawn - orbCount);  
            }
            
            currentTime = 0f;
        }
    }

    public int DecreaseOrbs()
    {
        currentTime = 0f;
        return --orbCount;
    }

    private void Spawn(int amt)
    {
        if (amt < 1)
        {
            Utility.Quit();
            throw new ArgumentException("Spawn() arg cannot be less than 1");
        }

        for (int i = 0; i < amt; i++)
        {
            int num = UnityEngine.Random.Range(0, locations.Length);
            float xFactor = locations[num].localScale.x / 2;
            float yFactor = locations[num].localScale.y / 2;

            float xUpperBound = locations[num].position.x + xFactor;
            float xLowerBound = locations[num].position.x - xFactor;

            float yUpperBound = locations[num].position.y + yFactor;
            float yLowerBound = locations[num].position.y - yFactor;

            float x = UnityEngine.Random.Range(xUpperBound, xLowerBound);
            float y = UnityEngine.Random.Range(yUpperBound, yLowerBound);

            Instantiate(orbPrefab, new Vector2(x, y), Quaternion.identity);
            orbCount++;
        }
    }
}
