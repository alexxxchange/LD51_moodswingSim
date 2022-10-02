using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class enemies : MonoBehaviour
{

    public static enemies instance;
    public List<enemy> activeEnemies = new List<enemy>();
    WaitForSeconds tickRate = new WaitForSeconds(1.1f);
    public bool shouldTick;
    Transform player;
    public Transform[] scatterPositions;

    [SerializeField] Vector3 northOffset;
    [SerializeField] Vector3 southOffset;
    [SerializeField] Vector3 eastOffset;
    [SerializeField] Vector3 westOffset;

    [Header("more enemies")]
    [SerializeField] GameObject[] auxEnemies;
    private void Awake()
    {
      if(instance == null)  instance = this;
    }
    void Start()
    {
        shouldTick = true;

        StartCoroutine(Tick());
        player = Player.instance.transform;

        if (PlayerPersistance.instance.difficulty >= 2)
        {
            foreach (var e in auxEnemies)
            {
                e.SetActive (true);
            }
        }

    }



    IEnumerator Tick()
    {
        while (shouldTick)
        {
            yield return tickRate;
            if (!Player.instance.scatter) EnemiesAggregateChase();
        }
    }

    public void PickScatterPoints()
    {
        if (activeEnemies.Count > 0)
        {

            for (int i = 0; i < activeEnemies.Count; i++)
            {
                activeEnemies[i].PlayScatterAnim();
                activeEnemies[i].rePathing = false;
                //
                if (i > scatterPositions.Length - 1)
                {
                    int randomIndex = Random.Range(0, scatterPositions.Length);
                    activeEnemies[i].scatterPosition = scatterPositions[randomIndex].position;
                    activeEnemies[i].agent.SetDestination(scatterPositions[randomIndex].position);
                }
                else
                {
                    activeEnemies[i].scatterPosition = scatterPositions[i].position;
                    activeEnemies[i].agent.SetDestination(scatterPositions[i].position);
                }
         
            }

        }
    }

    public void EnemiesAggregateChase()
    {
        if (activeEnemies.Count > 0)
        {
            enemy closest = activeEnemies.OrderBy(go => (player.position - go.transform.position).sqrMagnitude).First();
            closest.offsetPosition = Vector3.zero;


            for (int i = 1; i < activeEnemies.Count; i++)
            {
                float randomValX = UnityEngine.Random.Range(-4, 4);
                float randomValZ = UnityEngine.Random.Range(-4, 4);
                var randomOffset = new Vector3(randomValX, 0, randomValZ);
                activeEnemies[i].offsetPosition = randomOffset;
                print("enemy_" + activeEnemies[i].name + "offset " + randomOffset);
            }
        }
    }

}
