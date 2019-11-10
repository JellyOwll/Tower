using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : EnemyState
{
    [SerializeField]
    protected List<Transform> checkPointTransform = new List<Transform>();
    protected  NavMeshAgent agent;
    protected int index;
    protected Transform target;
    protected int searchIndex;
    [SerializeField]
    protected int searchIndexMax = 5;
    protected float life;
    [SerializeField]
    protected float Maxlife = 100;
    public GameObject BloodPrefab;

    public float Life
    {
        get => life;
        set
        {
            life = value;
            CheckLife();
        }
    }

    private void CheckLife()
    {
        if(Life <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        life = Maxlife;
        agent = GetComponent<NavMeshAgent>();
    }

    protected void SearchPlayer()
    {
        if(PointInsideSphere(FindObjectOfType<Player>().transform.position, transform.position, 15))
        {
            Vector3 direction = FindObjectOfType<Player>().transform.position - transform.position;
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.GetComponent<Player>())
                {
                    target = hit.collider.GetComponent<Player>().transform;
                    enemyState = State.chase;
                }
                else
                {
                    if (enemyState == State.chase)
                    {
                        enemyState = State.search;
                    }
                    else if (enemyState == State.patrol)
                    {
                        enemyState = State.patrol;
                    }
                }
            }
        }
    }

    protected bool PointInsideSphere(Vector3 point, Vector3 center, float radius)
    {
        return Vector3.Distance(point, center) < radius;
    }
        private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (enemyState == State.chase)
            {
                enemyState = State.search;
            }
            else if (enemyState == State.patrol)
            {
                enemyState = State.patrol;
            }
        }
    }

    protected override void DoActionChase()
    {
        SearchPlayer();
        if (agent.destination != target.position)
        {
            agent.destination = target.position;
        }
    }
    protected override void DoActionSearch()
    {
        SearchPlayer();
        if (agent.destination == target.position || Vector3.Distance(transform.position, agent.destination) <= 2f)
        {
            agent.destination = RandomNavSphere(transform.position, 10, -1);
            searchIndex++;
            if(searchIndex >= searchIndexMax)
            {
                searchIndex = 0;
                enemyState = State.patrol;
            }
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    protected override void DoActionPatrol()
    {
        SearchPlayer();
        if (checkPointTransform.Count != 0)
        {
            if(agent.destination != checkPointTransform[index].position)
            {
                agent.destination =checkPointTransform[index].position;
            }
            if (Vector3.Distance(transform.position, agent.destination) <= 2f)
            {
                index++;
                if (index >= checkPointTransform.Count)
                {
                    index = 0;
                }
            }
        }
    }

    
}
