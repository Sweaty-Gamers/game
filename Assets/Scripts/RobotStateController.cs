using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStateController : MonoBehaviour, EnemyStateController
{
    Animator animator;
    EnemyPathfindScript robot;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        robot = GetComponent<EnemyPathfindScript>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void UpdateState()
    {

        if (robot.isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (robot.isAttacking)
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }
}
