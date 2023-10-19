using UnityEngine;
using UnityEngine.AI;

public class FlyingDragonScript : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public EnemyStateController enemy;
    public bool isWalking;
    public bool isAttacking;
    public float rotationSpeed = 5f;
    public float circlingRadius = 50f;
    public float stoppingDistance = 50f;
    public float tangentDistance = 50f; // Adjust the tangent distance based on your requirements
    public GameObject flamethrowerPrefab; // Reference to the flamethrower prefab
    public GameObject activeFire;
    public GameObject activeFire2;
    public float flamethrowerRange = 20f; // Set the range at which the flamethrower can be used
    public Transform spawnPoint; // Reference to the spawn point empty GameObject
    public Transform spawnPoint2; // Reference to the spawn point empty GameObject

    private float timeSinceLastDestinationChange;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<EnemyStateController>() ?? GetComponentInChildren<EnemyStateController>();
        timeSinceLastDestinationChange = 0f;

        // Set an initial random destination
        SetRandomDestinationAroundPlayer();
    }

    void Update()
    {
        // Update the time since the last destination change
        timeSinceLastDestinationChange += Time.deltaTime;

        // Check the distance between the dragon and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If within the stopping distance, move along a tangent line
        if (distanceToPlayer <= stoppingDistance)
        {
            MoveAlongTangentLine();
            isAttacking = true;
            isWalking = false;


            //SpawnFlamethrower();
 
        }
        else
        {
            // Rotate towards the player
            Vector3 directionToPlayer = player.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move towards the current destination
            agent.isStopped = false;
            isAttacking = false;
            isWalking = true;
            agent.destination = player.position;
        }

        UpdateStateTransition();
    }

    void SpawnFlamethrower()
    {
        Debug.Log("testttttttttttt");

        // Instantiate the flamethrower prefab at the spawn point's position and rotation
        Quaternion rotation = Quaternion.Euler(45f, spawnPoint.rotation.eulerAngles.y, spawnPoint.rotation.eulerAngles.z);
        this.activeFire = Instantiate(flamethrowerPrefab, spawnPoint.position, rotation);
        this.activeFire.transform.SetParent(transform);
        this.activeFire.SetActive(true);

       
        // Use the Invoke method to spawn the second flamethrower after a delay
        float delay = 0.35f; // Delay in seconds
        Invoke("SpawnSecondFlamethrower", delay);
    }

    void SpawnSecondFlamethrower()
    {
        Quaternion rotation = Quaternion.Euler(45f, spawnPoint.rotation.eulerAngles.y, spawnPoint.rotation.eulerAngles.z);
        this.activeFire2 = Instantiate(flamethrowerPrefab, spawnPoint2.position , rotation);
        this.activeFire2.transform.SetParent(transform);
        this.activeFire2.SetActive(true);
    }


    void StopFlamethrower()
    {
        this.activeFire.SetActive(false);
    }

    float GetRandomChangeDestinationInterval()
    {
        return Random.Range(10f, 20f); // Adjust the interval range based on your requirements
    }

    void SetRandomDestinationAroundPlayer()
    {
        // Calculate a random point within the circling radius around the player
        Vector2 randomOffset = Random.insideUnitCircle * circlingRadius;
        Vector3 randomDestination = player.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

        // Set the destination for the NavMeshAgent
        agent.destination = randomDestination;

        // Reset the timer
        timeSinceLastDestinationChange = 0f;
    }

    void MoveAlongTangentLine()
    {
        // Calculate the direction from the player to the dragon
        Vector3 directionToDragon = transform.position - player.position;

        // Calculate a tangent direction
        Vector3 tangentDirection = Vector3.Cross(directionToDragon.normalized, Vector3.up);

        // Calculate the point on the tangent line outside the stopping distance
        Vector3 tangentPoint = player.position + tangentDirection * tangentDistance;

        // Set the destination for the NavMeshAgent
        agent.destination = tangentPoint;

        // Reset the timer
        timeSinceLastDestinationChange = 0f;
    }

    void UpdateStateTransition()
    {
        enemy.UpdateState();
    }
}
