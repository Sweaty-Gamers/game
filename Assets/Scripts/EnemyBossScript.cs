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
    public float jumpCooldown = 7f;
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
    public Boss bossScript;

    private Vector3 initialPlayerPosition;

    // Adjust these force values based on the desired pushback strength
    public float pushbackForce = 30f;
    public float upwardForce = 2f;
    public float maxPlayerHeight = 20f;

    // Flag to track if the player is currently colliding with the boss
    private bool isCollidingWithPlayer = false;


    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        agent = GetComponent<NavMeshAgent>();
        navMeshLink = GetComponent<NavMeshLink>();
        bossScript = GetComponent<Boss>();
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
                //canCharge = false;
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
                canCharge = true;
                isCharge = false;
                ChargeAttack();
            }

            navMeshLink.startPoint = transform.position;
            navMeshLink.endPoint = player.position;

            // Activate the NavMeshLink to make it valid for pathfinding
            navMeshLink.enabled = true;

            if (Vector3.Distance(transform.position, player.position) < 2f)
            {
                Vector3 pushDirection = (transform.position - player.position).normalized;
                pushDirection.y = 0f; // Ensure no vertical component

                // Apply the push force
                float pushForce = 100f; // Adjust this value based on your needs
                player.GetComponent<Rigidbody>().AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }

            if (!IsPlayerInRange())
            {
                MoveTowardsPlayer();
                agent.speed = 7f;
                agent.acceleration = 20f;
            }

            this.UpdateStateTransition();
            return;
        }

        if (!IsPlayerInRange())
        {
            MoveTowardsPlayer();
            agent.speed = 7f;
            agent.acceleration = 100f;
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
        else
        {
            MoveTowardsPlayer();
            agent.speed = 7f;
            agent.acceleration = 20f;
        }

        player.position = new Vector3(player.position.x, Mathf.Clamp(player.position.y, 0f, maxPlayerHeight), player.position.z);

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
            agent.speed = 30f;

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
            //Debug.Log(Vector3.Distance(transform.position, player.position));
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;

            // Get the Rigidbody component of the player
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            // Check if the player has a Rigidbody
            if (playerRigidbody != null)
            {
                // Calculate the pushback direction (opposite to the collision normal)
                Vector3 pushbackDirection = -collision.contacts[0].normal;

                // Apply the pushback force to the player
                playerRigidbody.AddForce(pushbackDirection * 300f, ForceMode.Impulse);

                // Apply an additional upward force to lift the player a bit
                playerRigidbody.AddForce(Vector3.up * 1000f, ForceMode.Impulse);
                Debug.Log("force");
                PreventPlayerThroughFloor(playerRigidbody);
            }
        }
            
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }

    void FixedUpdate()
    {
        // Check if the player is currently colliding with the boss
        if (isCollidingWithPlayer)
        {
            Rigidbody playerRigidbody = GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                Vector3 pushbackDirection = -transform.forward; // Example: pushback in the opposite direction of the boss

                // Apply the pushback force to the player
                playerRigidbody.AddForce(Vector3.back * pushbackForce, ForceMode.Impulse);

                // Apply an additional upward force to lift the player a bit
                playerRigidbody.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
                PreventPlayerThroughFloor(playerRigidbody);
            }
        }
    }

    void PreventPlayerThroughFloor(Rigidbody playerRigidbody)
    {
        // Raycast to check for the ground beneath and to the left of the player
        RaycastHit hit;
        float raycastDistance = 1.0f; // Adjust this value based on your player's size
        float offset = 0.1f; // Adjust this value to set the player just above the ground

        Vector3 downDirection = Vector3.down;
        Vector3 leftDirection = Vector3.left;

        if (Physics.Raycast(playerRigidbody.position, downDirection, out hit, raycastDistance))
        {
            // Check if the boss is above the player
            if (transform.position.y > playerRigidbody.position.y)
            {
                // Adjust the player's position just above the ground
                playerRigidbody.position = new Vector3(playerRigidbody.position.x, Mathf.Min(hit.point.y + offset, maxPlayerHeight), playerRigidbody.position.z);

                // Push the player out from under the boss in the horizontal direction
                float pushForce = 10f; // Adjust this value based on your needs
                playerRigidbody.AddForce(leftDirection * pushForce, ForceMode.Impulse);
            }
        }
    }


}

