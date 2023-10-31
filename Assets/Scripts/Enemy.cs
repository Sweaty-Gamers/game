using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{

    public NavMeshAgent agent;
    public EnemyPathfindScript pathFindScript;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pathFindScript = GetComponent<EnemyPathfindScript>();
    }


}
