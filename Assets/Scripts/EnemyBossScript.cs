using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBossScript : MonoBehaviour
{
    public Transform player;
    public float chargeSpeed = 200f;
    public float jumpForce = 10f;
    public float attackDistance = 70f;  //change later
    public float chargeCooldown = 5f;
    public float jumpCooldown = 8f;
    public float meleeCooldown = 2f;

    private bool canCharge = true;
    private bool canJump = true;
    private bool canMelee = true;

    public NavMeshAgent agent;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        agent = GetComponent<NavMeshAgent>();
        //agent.speed = 40f;
    }

    void Update()
    {
        if (!IsPlayerInRange())
        {
            MoveTowardsPlayer();
        }
        else if (canCharge)
        {
            ChargeAttack();
        }
        /*else if (canJump)
        {
            JumpAttack();
        }
        else if (canMelee)
        {
            MeleeAttack();
        } */
    }

    private void MoveTowardsPlayer()
    {
        // Set the destination for the NavMeshAgent to the player's position
        if (player != null)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    private void ChargeAttack()
    {

        // Check if the player is in range
        if (IsPlayerInRange())
        {
            // Perform charge attack
            Debug.Log("Charge Attack!");
            agent.speed = 150f;
            // Calculate the direction from the enemy to the player
            Vector3 chargeDirection = (player.position - transform.position).normalized;

            // Set the NavMeshAgent's destination to the player's position
            agent.SetDestination(player.position);

            // Increase the NavMeshAgent's acceleration for the charge attack
            agent.acceleration = 50f; // You can adjust this value to control acceleration

            // Start the charge cooldown coroutine
            canCharge = false;
            StartCoroutine(ChargeCooldown());
        }

    }


    private void JumpAttack()
    {
        // Check if the player is in range
        if (IsPlayerInRange())
        {
            // Perform jump attack
            Debug.Log("Jump Attack!");
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
            StartCoroutine(JumpCooldown());
        }
    }

    private void MeleeAttack()
    {
        // Check if the player is in range
        if (IsPlayerInRange())
        {
            // Perform melee attack
            Debug.Log("Melee Attack!");
            canMelee = false;
            StartCoroutine(MeleeCooldown());
        }
    }

    private bool IsPlayerInRange()
    {
        if (player != null)
        {
            Debug.Log(Vector3.Distance(transform.position, player.position) < attackDistance);
            Debug.Log(Vector3.Distance(transform.position, player.position));
            Debug.Log(attackDistance);
            return Vector3.Distance(transform.position, player.position) < attackDistance;
        }

        return false;
    }

    private IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(chargeCooldown);
        canCharge = true;
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private IEnumerator MeleeCooldown()
    {
        yield return new WaitForSeconds(meleeCooldown);
        canMelee = true;
    }
}
