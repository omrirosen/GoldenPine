using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject spawnerRight;
    [SerializeField] private GameObject spawnerLeft;
    [SerializeField] private float timeToSpawn = 0;
    [SerializeField] private float randomSpawnTime;
    public int numbOfEnemies;
    [SerializeField] private int randomSpawn;
    

    private void Awake()
    {
        spawnerRight = GameObject.Find("Spawner Right");
        spawnerLeft = GameObject.Find("Spawner Left");
        randomSpawnTime = 3f;
    }

    private void Update()
    {
        if (numbOfEnemies < 4)
        {
            timeToSpawn += Time.deltaTime;
        }
        
        CreateUnihog();
    }

    private void CreateUnihog()
    {
        
        if (numbOfEnemies < 4 && timeToSpawn > randomSpawnTime)
        {
            timeToSpawn = 0f;
            randomSpawnTime = Random.Range(2.5f, 4f);
            randomSpawn = Random.Range(1, 3);
            if (randomSpawn == 1)
            {
                Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnerRight.transform.position, Quaternion.identity, spawnerRight.transform.parent);
                enemyPrefab[0].transform.localScale = spawnerRight.transform.localScale;
                enemyPrefab[1].transform.localScale = spawnerRight.transform.localScale;
               
            }
            else
            {
                Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnerLeft.transform.position, Quaternion.identity,spawnerLeft.transform.parent);
                enemyPrefab[0].transform.localScale = spawnerLeft.transform.localScale;
                enemyPrefab[1].transform.localScale = spawnerLeft.transform.localScale;
            }
            
        }
    }
}
