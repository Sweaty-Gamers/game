using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfindScript : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    public bool isWalking;
    public bool isAttacking;
    public EnemyStateController enemy;
    //public MinotaurStateController minotaur;

    public float stoppingDistance = 15f; // Adjust this distance based on your requirements
    // make one for ranged soon

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<EnemyStateController>() ?? GetComponentInChildren<EnemyStateController>();
        //enemy = new MinotaurStateController();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log(distanceToPlayer);


        if (distanceToPlayer > stoppingDistance)
        {
            agent.isStopped = false; // Stop the NavMeshAgent
            agent.destination = player.position;
            isWalking = true;
            isAttacking = false;
        }
        else
        {
            // Stop moving
            Debug.Log("stoped");
            isWalking = false;
            agent.isStopped = true; // Stop the NavMeshAgent
            isAttacking = true;

            // Set a new destination to the current position to ensure immediate stopping
            agent.destination = transform.position;
        }

        this.UpdateStateTransition();
    }

    void UpdateStateTransition()
    {
        enemy.UpdateState();
    }
}
