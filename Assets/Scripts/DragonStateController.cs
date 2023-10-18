using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonStateController : MonoBehaviour, EnemyStateController
{
    Animator animator;
    EnemyPathfindScript dragon;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        dragon = GetComponent<EnemyPathfindScript>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void UpdateState()
    {

        if (dragon.isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (dragon.isAttacking)
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }
}
