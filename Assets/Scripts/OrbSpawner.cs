using System;
using UnityEngine;
using GeneralUtility;

public class OrbSpawner : MonoBehaviour
{
    [Header("Boundaries")]
    [SerializeField]
    private Vector2 topLeft;

    [SerializeField]
    private Vector2 botRight;

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

        if (orbCount > maxOrbSpawn)
        {
            Utility.Quit();
            throw new ArgumentException("Child Orbs in OrbSpawner exceeded maxOrbSpawn");
        }

        if (initialOrbSpawn > 0)
        {
            Spawn(initialOrbSpawn);
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
            float x = UnityEngine.Random.Range(topLeft.x, botRight.x);
            float y = UnityEngine.Random.Range(topLeft.y, botRight.y);

            Instantiate(orbPrefab, new Vector2(x, y), Quaternion.identity);
            orbCount++;
        }
    }
}
