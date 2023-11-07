using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Entity
{

    public NavMeshAgent agent;
    public EnemyPathfindScript pathFindScript;
    public float dropRate;
    public GameObject medkit;
    public GameObject nuke;
    public GameObject ammo;
    public GameObject[] consumables;

    public void Start()
    {
        consumables = new GameObject[] { medkit, nuke, ammo };
        agent = GetComponent<NavMeshAgent>();
        pathFindScript = GetComponent<EnemyPathfindScript>();
    }

    public abstract void DropConsumable();
}
