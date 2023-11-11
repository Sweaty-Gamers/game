using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBossScript : MonoBehaviour
{
    public Transform player;
    public float chargeSpeed = 15f;
    public float jumpForce = 500f;
    public float attackDistance = 50f;  //change later
    public float chargeCooldown = 2f;
    public float jumpCooldown = 6f;
    public float meleeCooldown = 2f;
    public float jumpHeight = 2f;
    public float jumpDuration = 0.5f;
    public float meleeRange = 4f;
    public float rotationSpeed = 5f;

    private bool startedJumpCoolDown = false;
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

    private Vector3 initialPlayerPosition;


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
        if (isCharge)
        {
            if (!startedJumpCoolDown)
            {
                StartCoroutine(JumpCooldown());
                startedJumpCoolDown = true;
            }
            if (IsMeleeRange()) {
                MeleeAttack();
                agent.speed = 7f;
                agent.acceleration = 20f;
                isCharge = false;
                canCharge = false;
            }
            if (canJump)
            {
                Debug.Log("huh??");
                Vector3 directionToPlayer = player.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

                // Set the rotation immediately
                agent.transform.rotation = targetRotation;
                JumpAttack();
                agent.speed = 7f;
                agent.acceleration = 20f;
                canJump = false;
            }

            navMeshLink.startPoint = transform.position;
            navMeshLink.endPoint = player.position;

            // Activate the NavMeshLink to make it valid for pathfinding
            navMeshLink.enabled = true;

            if (Vector3.Distance(transform.position, player.position) < 1f)
            {
                Vector3 pushDirection = (transform.position - player.position).normalized;
                pushDirection.y = 0f; // Ensure no vertical component

                // Apply the push force
                float pushForce = 100f; // Adjust this value based on your needs
                player.GetComponent<Rigidbody>().AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }

            this.UpdateStateTransition();
            return;
        }

        if (!IsPlayerInRange() && !isCharge)
        {
            MoveTowardsPlayer();
            agent.speed = 7f;
            agent.acceleration = 20f;
        }

        else if (IsMeleeRange())
        {
            MeleeAttack();
            agent.speed = 7f;
            agent.acceleration = 20f;
        }
        else if (canCharge)
        {
            ChargeAttack();
            isCharge = true;
            canJump = false;
        }
        else if (canJump && !isCharge)
        {
            JumpAttack();
            agent.speed = 7f;
            agent.acceleration = 20f;
        }

        else
        {
            MoveTowardsPlayer();
            agent.speed = 7f;
            agent.acceleration = 20f;
        }
        

        // Set the start and end positions of the NavMeshLink
        navMeshLink.startPoint = transform.position;
        navMeshLink.endPoint = player.position;

        // Activate the NavMeshLink to make it valid for pathfinding
        navMeshLink.enabled = true;

        if (Vector3.Distance(transform.position, player.position) < 1f)
        {
            Vector3 pushDirection = (transform.position - player.position).normalized;
            pushDirection.y = 0f; // Ensure no vertical component

            // Apply the push force
            float pushForce = 100f; // Adjust this value based on your needs
            player.GetComponent<Rigidbody>().AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }

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

            Vector3 directionToPlayer = player.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate the NavMeshAgent towards the player
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


    private void ChargeAttack()
    {
        // Check if the player is in range
        // Check if the player is in range
        if (IsPlayerInRange())
        {
            isWalking = true;
            isAttacking = false;
            isCharge = true;

            // Store the initial player position when starting the charge

            // Perform charge attack
            Debug.Log("Charge Attack!");
            agent.speed = 20f;

            // Calculate the destination point slightly past the player's position
            Vector3 chargeDirection = (player.position - transform.position).normalized;
            Vector3 chargeDestination = player.position + chargeDirection * (attackDistance + 5f);

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
            agent.speed = 7f; // Set the default speed
            agent.acceleration = 20f; // Set the default acceleration
            agent.ResetPath(); // Clear the current path
            isCharge = false;
        }
    }



    private void JumpAttack()
    {
        // Check if the player is in range
        if (IsPlayerInRange())
        {
            isWalking = false;
            isAttacking = false;

            Vector3 directionToPlayer = player.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate the NavMeshAgent towards the player
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Calculate the direction from the enemy to the player
            Vector3 jumpDirection = (player.position - transform.position).normalized;

            // Calculate the destination point slightly above the player's position
            Vector3 jumpDestination = player.position + jumpDirection * 10f; // Adjust jump distance as needed

            // Set the NavMeshAgent's destination to the calculated jump destination
            agent.SetDestination(jumpDestination);


            // Start the jump cooldown coroutine
            canJump = false;
            //StartCoroutine(JumpCooldown());
            Debug.Log("Jump attack");
            // Start the parabola jump coroutine
            StartCoroutine(Parabola2(agent, jumpHeight, jumpDuration)); // Adjust duration as needed
        }
    }

    // Check if the player is on the same ground level (y-axis)
    private bool IsOnSameGroundLevel()
    {
        float groundCheckDistance = 0.1f; // Adjust this value based on your scene
        return Mathf.Abs(player.position.y - transform.position.y) < groundCheckDistance;
    }


    private void MeleeAttack()
    {
        // Check if the player is in range
        if (IsPlayerInRange())
        {
            Vector3 directionToPlayer = player.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate the NavMeshAgent towards the player
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            agent.isStopped = true;
            agent.destination = transform.position;  //make sure to stop

            isWalking = false;
            isAttacking = true;

            // Perform melee attack
            Debug.Log("Melee Attack!");
            if (Vector3.Distance(transform.position, player.position) < 1f)
            {
                // Apply continuous push force if the player is in melee range and on the same ground level
                Vector3 pushDirection = (transform.position - player.position).normalized;
                pushDirection.y = 0f; // Ensure no vertical component

                // Apply the push force
                float pushForce = 100f; // Adjust this value based on your needs
                Debug.Log("testtt");
                player.GetComponent<Rigidbody>().AddForce(pushDirection * pushForce * Time.deltaTime, ForceMode.Impulse);
            }
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

    private bool IsPlayerInJumpRange()
    {
        if (player != null)
        {
            //Debug.Log(Vector3.Distance(transform.position, player.position) < attackDistance);
            //Debug.Log(Vector3.Distance(transform.position, player.position));
            //Debug.Log(attackDistance);
            return Vector3.Distance(transform.position, player.position) < 25f;
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
        startedJumpCoolDown = false;
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
