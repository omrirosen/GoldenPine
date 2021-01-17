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
    public int enemiesDefeated;
    [SerializeField]private int maxNumOfEnemies = 1;
    [SerializeField]private ScoreManager scoreManager;
    [SerializeField]private int maxSpawnSize = 1;
    private void Awake()
    {
        spawnerRight = GameObject.Find("Spawner Right");
        spawnerLeft = GameObject.Find("Spawner Left");
        scoreManager = FindObjectOfType<ScoreManager>();
        randomSpawnTime = 3f;
    }

    private void Update()
    {
        if (numbOfEnemies < maxNumOfEnemies)
        {
            timeToSpawn += Time.deltaTime;
        }
        
        CreateUnihog();
        IncreasMax();
    }

    private void CreateUnihog()
    {
        
        if (numbOfEnemies < maxNumOfEnemies && timeToSpawn > randomSpawnTime)
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

    private void IncreasMax()
    {
        if(enemiesDefeated == maxSpawnSize)
        {
            maxNumOfEnemies++;
            maxSpawnSize++;
            enemiesDefeated = 0;
        }
    }
    
    public void IncreasScore()
    {
        scoreManager.AddToScore();
    }
}
