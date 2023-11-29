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
    private bool isDead;

    public void Start()
    {
        consumables = new GameObject[] { medkit, nuke, ammo };
        agent = GetComponent<NavMeshAgent>();
        pathFindScript = GetComponent<EnemyPathfindScript>();
        isDead = false;
    }

    public void DropConsumable()
    {
        float drop = Random.Range(0f, 1f);
        Debug.Log(drop);
        if (drop <= dropRate)
        {
            Instantiate(consumables[Random.Range(0, consumables.Length)], new Vector3(transform.position.x, Mathf.Max(transform.position.y, 0.5f), transform.position.z), Quaternion.identity);
        }
    }

    public override void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        DropConsumable();
        base.Die();
    }
}
