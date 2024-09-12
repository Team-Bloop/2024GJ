using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using GeneralUtility;

public class OrbSpawner : MonoBehaviour
{
    const int MAX_SPAWN_ATTEMPTS = 10;

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

    [SerializeField]
    private bool showLocationSquares;

    private int orbCount;
    private float currentTime;
    private List<Transform> locations;

    private void Start()
    {
        orbCount = GetComponentsInChildren<Orb>().Length;
        currentTime = 0f;
        locations = new List<Transform>();

        Transform locationsTransform = transform.Find("Locations");
        locationsTransform.GetComponent<SortingGroup>().sortingOrder = showLocationSquares ? 0 : -2;

        foreach (Transform t in locationsTransform)
        {
            locations.Add(t);
        }

        if (locations.Count == 0)
        {
            Utility.Quit();
            throw new ArgumentException("OrbSpawner -> Locations has no children (needs 2D square sprites).");
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
        int spawnAttempts = 0;
        List<Collider2D> results = new List<Collider2D>();

        if (amt < 1)
        {
            Utility.Quit();
            throw new ArgumentException("Spawn() arg cannot be less than 1");
        }

        for (int i = 0; i < amt; i++)
        {
            int num = UnityEngine.Random.Range(0, locations.Count);
            float xFactor = locations[num].localScale.x / 2;
            float yFactor = locations[num].localScale.y / 2;

            float xUpperBound = locations[num].position.x + xFactor;
            float xLowerBound = locations[num].position.x - xFactor;

            float yUpperBound = locations[num].position.y + yFactor;
            float yLowerBound = locations[num].position.y - yFactor;

            float x = UnityEngine.Random.Range(xUpperBound, xLowerBound);
            float y = UnityEngine.Random.Range(yUpperBound, yLowerBound);

            Vector2 location = new Vector2(x, y);
            bool canSpawn = true;

            Physics2D.OverlapBox(location, Vector2.one, 0f, new ContactFilter2D().NoFilter(), results);
            
            foreach (Collider2D item in results)
            {
                if (item != null && (item.tag == "Orb" || item.tag == "Player" || item.tag == "Storm" || item.tag == "CollisionTiles"))
                {
                    canSpawn = false;
                    break;
                }
            }

            if (canSpawn)
            {
                Instantiate(orbPrefab, location, Quaternion.identity);
                orbCount++;
            } else
            {
                i--;

                if (++spawnAttempts >= MAX_SPAWN_ATTEMPTS)
                {
                    Debug.LogWarning($"Max spawn attempts ({MAX_SPAWN_ATTEMPTS}) reached, exiting Spawn() loop");
                    break;
                }
            }
        }
    }
}
