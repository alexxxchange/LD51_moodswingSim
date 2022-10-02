using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class enemy : MonoBehaviour
{
  
    public NavMeshAgent agent;
    public Vector3 offsetPosition = Vector3.zero;
    public Vector3 scatterPosition;
    WaitForSeconds tickRate = new WaitForSeconds(0.2f);
    public bool shouldTick;
    Transform player;
    public EnemyState currentState;
    [SerializeField] GameObject enemyGraphics;
    [SerializeField] GameObject deathPrefab;
    [SerializeField] GameObject dropItem;

    [SerializeField] Animator anim;
    public float moveSpeed;
    public bool rePathing;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = agent.speed + PlayerPersistance.instance.difficulty;
        if (moveSpeed > 6) moveSpeed = 6;

        agent.speed = moveSpeed;
        shouldTick = true;

        StartCoroutine(Tick());
        player = Player.instance.transform;
        SwitchEnemyState(EnemyState.chase);
        if (!enemies.instance.activeEnemies.Contains(this)) enemies.instance.activeEnemies.Add(this);
    }

    IEnumerator Tick()
    {
        while (shouldTick)
        {
            yield return tickRate;
  
            AI_Update();
        }
    }

    void AI_Update()
    {
        switch (currentState)
        {
            case EnemyState.chase:
                {
                    if (Vector3.Distance(transform.position, player.position) > 0.1f)
                    {
                        agent.SetDestination(player.position + offsetPosition);
                    }
                    break;
                }
      
            case EnemyState.scatter:
                {
                    if (Vector3.Distance(transform.position, scatterPosition) <= 0.2f)
                    {
                        PickNewPoint();
                        rePathing = false;
                    }

                    //if (Vector3.Distance(transform.position, scatterPosition) > 0.2f)
                    //{
                    //    agent.SetDestination(scatterPosition);
                    //}
                    if (!rePathing && Vector3.Distance(transform.position, player.position) < 6)
                    {
                        var dir = (transform.position - player.position).normalized;
                        dir.y = 0; //flatten
                        scatterPosition = dir * 10;
                        agent.SetDestination(scatterPosition);
                        rePathing = true;
                    }

                
                    // avoidance?
                    break;
                }
            case EnemyState.dead:
                {
                    break;
                }

            default:
                break;
        }
    }

    private void PickNewPoint()
    {
        int randomIndex = Random.Range(0, enemies.instance.scatterPositions.Length);
        scatterPosition = enemies.instance.scatterPositions[randomIndex].position;
        agent.SetDestination(scatterPosition);
    }

    public void SwitchEnemyState(EnemyState newState)
    {
        currentState = newState;
    }

    public void Die()
    {

        //death stuff
        enemyGraphics.SetActive(false);
        GetComponent<Collider>().enabled = false;
        Instantiate(deathPrefab, transform.position, transform.rotation);
        Instantiate(dropItem, transform.position, transform.rotation);

        if (enemies.instance.activeEnemies.Contains(this)) enemies.instance.activeEnemies.Remove(this);
        if (enemies.instance.activeEnemies.Count == 0)
        {
            Player.instance.Win();
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent(out Player p);
        if (p != null)
        {
            p.Die();
        }
    }

    public void PlayScatterAnim()
    {
        anim.Play("Scatter");
    }
}

public enum EnemyState
{
    chase,
    scatter,
    dead,
}
