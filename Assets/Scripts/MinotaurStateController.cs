using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurStateController : MonoBehaviour, EnemyStateController
{
    Animator animator;
    EnemyPathfindScript minotaur;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        minotaur = GetComponent<EnemyPathfindScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void UpdateState()
    {

        if (minotaur.isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (minotaur.isAttacking)
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }
}
