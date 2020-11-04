using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGeneratorScript : MonoBehaviour
{
    [Header("Cloud Manager Caches")]
    [SerializeField] private GameObject parentCloudFolder;
    [SerializeField] private GameObject[] cloudPrefabs;
    [SerializeField] private GameObject endPoint;
    private Vector3 startPos;
    
    [Header("Cloud Spawning Config")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float minHeightYValue = -5.5f;
    [SerializeField] private float maxHeightYValue = 3f;
    [SerializeField] private float minCloudSpeed = 0.2f;
    [SerializeField] private float maxCloudSpeed = 2f;
    
    [Header("Prewarm Config")]
    [SerializeField] private float prewarmDistanceMultiplier = 4f;
    [SerializeField] private int numbOfPrewarmClouds = 35;
    

    private void Start()
    {
        startPos = transform.position;
        PreWarm();
        Invoke("AttemptSpawn", spawnInterval);
    }

    void SpawnCloud(Vector3 startPos)
    {
        GameObject cloud = Instantiate(cloudPrefabs[UnityEngine.Random.Range(0, cloudPrefabs.Length)]);
        cloud.transform.parent = parentCloudFolder.transform;
        

        startPos.y = UnityEngine.Random.Range(minHeightYValue, maxHeightYValue);
        cloud.transform.position = new Vector3(startPos.x, startPos.y, startPos.z);
        
        
        
        float randomSpeed = UnityEngine.Random.Range(minCloudSpeed, maxCloudSpeed);
        cloud.GetComponent<CloudScript>().StartFloating(randomSpeed, endPoint.transform.position.x);
    }
    

    void AttemptSpawn()
    {
        // check something
        SpawnCloud(startPos);
        Invoke("AttemptSpawn", spawnInterval);
    }

    void PreWarm()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 spawnPos = startPos + Vector3.right * (i * prewarmDistanceMultiplier);
            SpawnCloud(spawnPos);
        }
    }
}
