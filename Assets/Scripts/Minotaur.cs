using UnityEngine;

public class Minotaur : Enemy
{
    // Start is called before the first frame update
    public MeleeWeapon weapon;
    public float weaponDamage = 10f;
    public float knockBackForce = .4f;

    new void Start()
    {
        base.Start();
        weapon = gameObject.GetComponentInChildren<MeleeWeapon>();
        health = 400;
        maxHealth = 400;
        agent.speed = movementSpeed;
        dropRate = .5f;

        weapon.damage = weaponDamage;
        weapon.knockBack = knockBackForce;

        pathFindScript.stoppingDistance = 1.5f;
        pathFindScript.shootingDistance = 0;
    }
    public override void DropConsumable()
    {
        float drop = Random.Range(0f, 1f);
        Debug.Log(drop);
        if (drop > dropRate)
        {
            Instantiate(consumables[Random.Range(0, consumables.Length)], new Vector3(transform.position.x, Mathf.Max(transform.position.y, 0.5f), transform.position.z), Quaternion.identity);
        }
    }
    public override void Die()
    {
        DropConsumable();
        base.Die();
    }



}