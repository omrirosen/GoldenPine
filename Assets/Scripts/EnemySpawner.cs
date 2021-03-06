﻿using System;
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
    [SerializeField] private int maxNumOfEnemies = 1;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private int maxSpawnSize = 1;
    [SerializeField] private float maxSpawnTime = 4f;
    [SerializeField] private float minSpawnTime = 2.5f;
    private bool DecreasdSpawnTimer = false;
    private bool isOnRightTrigger;
    private void Awake()
    {
        spawnerRight = GameObject.Find("Spawner Right");
        spawnerLeft = GameObject.Find("Spawner Left");
        scoreManager = FindObjectOfType<ScoreManager>();
        randomSpawnTime = 3f;
        minSpawnTime = 2.5f;
        maxSpawnTime = 4f;
    }

    private void Update()
    {
        if (numbOfEnemies < maxNumOfEnemies)
        {
            timeToSpawn += Time.deltaTime;
        }
        
        if (scoreManager.currentScore %5 == 0 && scoreManager.currentScore !=0 && !DecreasdSpawnTimer)
        {
            DecreasSpawnTimer();
            DecreasdSpawnTimer = true;
        }
        
        if(scoreManager.currentScore %5 != 0)
        {
            DecreasdSpawnTimer = false;
        }
        CreateUnihog();
        IncreasMax();
    }

    private void CreateUnihog()
    {
        
        if (numbOfEnemies < maxNumOfEnemies && timeToSpawn > randomSpawnTime)
        {
            timeToSpawn = 0f;
            randomSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
           // randomSpawn = Random.Range(1, 3);
            if (!isOnRightTrigger)
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

    private void DecreasSpawnTimer()
    {
        if (minSpawnTime > 1f)
        {
            maxSpawnTime = maxSpawnTime - 0.2f;
            minSpawnTime = minSpawnTime - 0.2f;
        }
        
        if(minSpawnTime <= 1f)
        {
            minSpawnTime = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnRightTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnRightTrigger = false;
        }
    }
}
