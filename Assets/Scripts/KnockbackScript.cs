using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockbackScript : MonoBehaviour
{
    /// Seconds to slow enemy by when hit.
    public float slowTime;
    /// Factor to slow everything by.
    public float slowFactor = 10f;

    private float lastHitTime = -1;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Knockback() {
        float oldSpeed = navMeshAgent.speed;
        float oldAccel = navMeshAgent.acceleration;
        float oldAnimSpeed = animator.speed;
        
        navMeshAgent.speed = 0;
        navMeshAgent.acceleration = 0;
        navMeshAgent.velocity /= slowFactor;
        animator.speed /= slowFactor;

        yield return new WaitForSeconds(slowTime);

        navMeshAgent.speed = oldSpeed;
        navMeshAgent.acceleration = oldAccel;
        animator.speed = oldAnimSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (Time.time >= lastHitTime + slowTime) {
                lastHitTime = Time.time;
                StartCoroutine(Knockback());
            }
            
        }
    }
}
