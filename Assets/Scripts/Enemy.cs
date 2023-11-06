using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Entity
{

    public NavMeshAgent agent;
    public EnemyPathfindScript pathFindScript;
    public float dropRate;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pathFindScript = GetComponent<EnemyPathfindScript>();
    }

    public abstract void DropConsumable();
}
