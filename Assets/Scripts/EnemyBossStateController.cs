using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossStateController : MonoBehaviour, EnemyStateController
{
    Animator animator;
    EnemyBossScript boss;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boss = GetComponent<EnemyBossScript>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void UpdateState()
    {

        if (boss.isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (boss.isAttacking)
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }
}
