using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UniSpawner : MonoBehaviour
{
    [SerializeField] private GameObject uniPrefab;
    [SerializeField] private GameObject spawnerRight;
    [SerializeField] private GameObject spawnerLeft;
    [SerializeField] private float timeToSpawn = 0;
    [SerializeField] private float randomSpawnTime;
    public int numOfUnihogs;
    [SerializeField] private int randomSpawn;
    

    private void Awake()
    {
        spawnerRight = GameObject.Find("Spawner Right");
        spawnerLeft = GameObject.Find("Spawner Left");
        randomSpawnTime = 3f;
    }

    private void Update()
    {
        if (numOfUnihogs < 4)
        {
            timeToSpawn += Time.deltaTime;
        }
        
        CreateUnihog();
    }

    private void CreateUnihog()
    {
        
        if (numOfUnihogs < 4 && timeToSpawn > randomSpawnTime)
        {
            timeToSpawn = 0f;
            randomSpawnTime = Random.Range(2.5f, 4f);
            randomSpawn = Random.Range(1, 3);
            if (randomSpawn == 1)
            {
                Instantiate(uniPrefab, spawnerRight.transform.position, Quaternion.identity, spawnerRight.transform.parent);
                uniPrefab.transform.localScale = spawnerRight.transform.localScale;
            }
            else
            {
                Instantiate(uniPrefab, spawnerLeft.transform.position, Quaternion.identity,spawnerLeft.transform.parent);
                uniPrefab.transform.localScale = spawnerLeft.transform.localScale;
            }
            
        }
    }
}
