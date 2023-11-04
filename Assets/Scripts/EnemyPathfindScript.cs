using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfindScript : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public bool isWalking;
    public bool isAttacking;
    public EnemyStateController enemy;
    public EnemyShooting shootingScript;
    //public MinotaurStateController minotaur;

    public float rotationSpeed = 5f;

    public float stoppingDistance = 15f; // Adjust this distance based on your requirements
    public float shootingDistance = 30f;
    private float timeSinceLastShot = 0f;

    // make one for ranged soon

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<EnemyStateController>() ?? GetComponentInChildren<EnemyStateController>();
        shootingScript = GetComponent<EnemyShooting>();
        //enemy = new MinotaurStateController();

        string objectTag = gameObject.tag;
        //Debug.Log("Object Tag: " + objectTag);

        if (objectTag == "Enemy_Ranged")
        {
            stoppingDistance = 7f; //100f;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No GameObject with the tag 'Player' found.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //Debug.Log(distanceToPlayer);

        // Check if there is a clear line of sight to the player
        bool hasLineOfSight = HasLineOfSightToPlayer();
        //Debug.Log(hasLineOfSight);

        if (distanceToPlayer > stoppingDistance)
        {
            agent.isStopped = false; // Stop the NavMeshAgent
            agent.destination = player.position;
            isWalking = true;
            isAttacking = false;

            if (distanceToPlayer <= shootingDistance && gameObject.tag == "Enemy_Ranged")
            {
                timeSinceLastShot += Time.deltaTime;
                if (timeSinceLastShot >= 3f)
                {
                    shootingScript.Shoot(.33f); // Call the Shoot method when 3 seconds have passed
                    timeSinceLastShot = 0f; // Reset the timer
                }
            }
            else
            {
                //Debug.Log("WHAT?");
            }
        }
        else
        {
            // Stop moving
            //Debug.Log("stoped");
            //isWalking = false;
            //agent.isStopped = true; // Stop the NavMeshAgent
            //isAttacking = true;

            // Set a new destination to the current position to ensure immediate stopping
            
          
           
            // No valid path found, fallback to the original logic
            //RaycastHit hit;
            if (!HasLineOfSightToPlayer())
            {
                agent.isStopped = false;
                isWalking = true;
                isAttacking = false;
                agent.destination = player.position;
                // Calculate the path to the player
                //NavMeshPath path = new NavMeshPath();
                //agent.CalculatePath(player.position, path);
                //agent.SetPath(path);
                //Debug.Log("done");
                //Vector3 directionToPlayer = player.position - transform.position;
                //Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 40f);
            }
            else
            {
                //Debug.Log("no obstacle");
                // No obstacle detected, move towards the player
                //agent.isStopped = true;

                // Calculate the rotation towards the player
                Vector3 directionToPlayer = player.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

                // Smoothly rotate the NavMeshAgent towards the player
                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                isWalking = false;
                isAttacking = true;

  
                agent.isStopped = true;
                agent.destination = transform.position;  //make sure to stop
            }
            
        }

        this.UpdateStateTransition();
    }

    bool HasLineOfSightToPlayer()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Raycast to check for obstacles between the enemy and the player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, stoppingDistance))
        {
            // Check if the hit object is the player
            if (hit.collider.CompareTag("Player"))
            {
                // There is a clear line of sight to the player
                return true;
            }
            else
            {
                // Debug the tag of the object when it's not the player
                Debug.Log("Hit object's tag: " + hit.collider.tag);
            }
        }

        // There is an obstacle or no player in sight
        return false;
    }

    void UpdateStateTransition()
    {
        enemy.UpdateState();
    }
}
