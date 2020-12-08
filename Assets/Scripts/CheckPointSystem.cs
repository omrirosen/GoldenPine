using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSystem : MonoBehaviour
{
    public static CheckPointSystem instance;
    [SerializeField] GameObject checkpoint1;
    [SerializeField] GameObject checkpoint2;
    [SerializeField] GameObject playerRespawner;
    bool passedCP1 = false;
    

    private void Awake()
    {
        if (instance == null)
        {     
            instance = this;        
        }
        else
        {
            Destroy(gameObject);    
            return;                 
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("someone is in");
        if (collision.CompareTag("Player"))
        {
            print("its the Player");
            if (!passedCP1)
            {
                print("Saving Positon");
                passedCP1 = true;
                Instantiate(playerRespawner, checkpoint1.transform.position, Quaternion.identity);
                print("Saved!");
            }
            else
            {
                Instantiate(playerRespawner, checkpoint2.transform.position, Quaternion.identity);
            }
            
          
        }
    }


}
