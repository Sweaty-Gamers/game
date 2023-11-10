using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBossScript : MonoBehaviour
{
    public Transform player;
    public float chargeSpeed = 200f;
    public float jumpForce = 500f;
    public float attackDistance = 70f;  //change later
    public float chargeCooldown = 5f;
    public float jumpCooldown = 20f;
    public float meleeCooldown = 2f;
    public float jumpHeight = 50f;
    public float jumpDuration = 0.5f;
    public float meleeRange = 30.0f;

    private bool canCharge = true;
    private bool canJump = true;
    private bool canMelee = true;

    private bool isCharge = false;
    private bool isJump = false;
    private bool isMelee = false;

    public bool isWalking;
    public bool isAttacking;

    public EnemyStateController enemy;
    public NavMeshAgent agent;
    public NavMeshLink navMeshLink;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        agent = GetComponent<NavMeshAgent>();
        navMeshLink = GetComponent<NavMeshLink>();
        enemy = GetComponent<EnemyStateController>() ?? GetComponentInChildren<EnemyStateController>();

        //agent.speed = 40f;
    }

    void Update()
    {
        if (!IsPlayerInRange())
        {
            MoveTowardsPlayer();
        }
        else if (IsMeleeRange())
        {
            MeleeAttack();
        }
        else if (canCharge)
        {
            ChargeAttack();
        }
        else if (canJump && !isCharge)
        {
            JumpAttack();
        }
        else
        {
            MoveTowardsPlayer();
        }
        

        // Set the start and end positions of the NavMeshLink
        navMeshLink.startPoint = transform.position;
        navMeshLink.endPoint = player.position;

        // Activate the NavMeshLink to make it valid for pathfinding
        navMeshLink.enabled = true;

        this.UpdateStateTransition();
    }

    private void MoveTowardsPlayer()
    {
        // Set the destination for the NavMeshAgent to the player's position
        if (player != null)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            isWalking = true;
            isAttacking = false;
        }
    }

    private void ChargeAttack()
    {

        // Check if the player is in range
        if (IsPlayerInRange())
        {
            isWalking = true;
            isAttacking = false;

            // Perform charge attack
            Debug.Log("Charge Attack!");
            agent.speed = 150f;

            // Calculate the direction from the enemy to the player
            Vector3 chargeDirection = (player.position - transform.position).normalized;

            // Calculate the destination point slightly past the player's position
            //float chargeDistance = 50f; // Adjust this value to control how far past the player to charge
            Vector3 chargeDestination = player.position + chargeDirection * (attackDistance);

            // Set the NavMeshAgent's destination to the calculated destination point
            agent.SetDestination(chargeDestination);

            // Increase the NavMeshAgent's acceleration for the charge attack
            agent.acceleration = 50f; // You can adjust this value to control acceleration

            // Start the charge cooldown coroutine
            canCharge = false;
            StartCoroutine(ChargeCooldown());
        }
        else
        {
            // Player is out of range, reset agent values and destination
            agent.speed = 10f; // Set the default speed
            agent.acceleration = 20f; // Set the default acceleration
            agent.ResetPath(); // Clear the current path
        }

    }


    private void JumpAttack()
    {
        // Check if the player is in range
        if (IsPlayerInRange())
        {
            isWalking = false;
            isAttacking = false;

            // Calculate the direction from the enemy to the player
            Vector3 jumpDirection = (player.position - transform.position).normalized;

            // Calculate the destination point slightly above the player's position
            Vector3 jumpDestination = player.position + jumpDirection * 10f; // Adjust jump distance as needed

            // Set the NavMeshAgent's destination to the calculated jump destination
            agent.SetDestination(jumpDestination);

            // Start the jump cooldown coroutine
            canJump = false;
            StartCoroutine(JumpCooldown());

            // Start the parabola jump coroutine
            StartCoroutine(Parabola2(agent, jumpHeight, jumpDuration)); // Adjust duration as needed
        }
    }





    private void MeleeAttack()
    {
        // Check if the player is in range
        if (IsPlayerInRange())
        {
            agent.isStopped = true;
            agent.destination = transform.position;  //make sure to stop

            isWalking = false;
            isAttacking = true;

            // Perform melee attack
            Debug.Log("Melee Attack!");
            //canMelee = false;
            //StartCoroutine(MeleeCooldown());
        }
    }

    private bool IsPlayerInRange()
    {
        if (player != null)
        {
            //Debug.Log(Vector3.Distance(transform.position, player.position) < attackDistance);
            //Debug.Log(Vector3.Distance(transform.position, player.position));
            //Debug.Log(attackDistance);
            return Vector3.Distance(transform.position, player.position) < attackDistance;
        }

        return false;
    }

    private bool IsMeleeRange()
    {
        if (player != null)
        {
            //Debug.Log(Vector3.Distance(transform.position, player.position) < attackDistance);
            //Debug.Log(Vector3.Distance(transform.position, player.position));
            //Debug.Log(attackDistance);
            Debug.Log(Vector3.Distance(transform.position, player.position));
            return Vector3.Distance(transform.position, player.position) < meleeRange;
        }

        return false;
    }

    private IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(chargeCooldown);
        canCharge = true;
        isCharge = false;
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
        isJump = false;
    }

    private IEnumerator MeleeCooldown()
    {
        yield return new WaitForSeconds(meleeCooldown);
        canMelee = true;
        isMelee = false;
    }

    /*IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;

        // Raise the end position to avoid going through the ground
        Vector3 endPos = player.position + Vector3.up * agent.baseOffset + Vector3.up * height;

        float normalizedTime = 0.0f;

        while (normalizedTime < 1.15f)
        {
            // Adjust the yOffset to make the jump smoother
            float yOffset = height * Mathf.Sin(normalizedTime * Mathf.PI);

            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        // Jump is complete, reset the coroutine and NavMeshAgent
        agent.CompleteOffMeshLink();
    } */

    IEnumerator Parabola2(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = player.position; // Use the end position from OffMeshLinkData

        float normalizedTime = 0.0f;

        while (normalizedTime < 1.0f) // Adjust the loop condition
        {
            // Calculate the perfect parabola using Quadratic Bezier curve
            float yOffset = height * 4.0f * normalizedTime * (1.0f - normalizedTime);

            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        // Jump is complete, reset the coroutine and NavMeshAgent
        agent.CompleteOffMeshLink();
    }

    void UpdateStateTransition()
    {
        enemy.UpdateState();
    }



}
