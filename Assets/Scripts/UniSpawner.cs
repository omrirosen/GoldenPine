using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniSpawner : MonoBehaviour
{
    public int numOfUnihogs;
    [SerializeField] private GameObject uniPrefab;
    float timeToSpawn = 0;
    private void Update()
    {
        timeToSpawn += Time.deltaTime;
        CreateUnihog();
    }

    private void CreateUnihog()
    {
        if (numOfUnihogs < 4 && timeToSpawn >2f)
        {
            timeToSpawn = 0f;
            Instantiate(uniPrefab, transform.position, Quaternion.identity, transform.parent);
            uniPrefab.transform.localScale = transform.localScale;
        }
    }
}
