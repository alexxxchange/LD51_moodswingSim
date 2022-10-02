using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletPickupSpawners : MonoBehaviour
{

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject bulletPickupPrefab;
    float spawnCoolDownTimer;
    float spawnCoolDownTime;
  
    // Start is called before the first frame update
 
    // Update is called once per frame
    void Update()
    {

        
        if (Player.instance.bullets <= 0 && Time.time > spawnCoolDownTimer )
        {
  
           var childCount = transform.childCount;
            if (childCount > 0) return;
            else
            {
                int randomInt = Random.Range(0, spawnPoints.Length);
                Instantiate(bulletPickupPrefab, spawnPoints[randomInt].position, spawnPoints[randomInt].rotation);
                spawnCoolDownTimer = Time.time + spawnCoolDownTime;
            }
        }

    }
}
