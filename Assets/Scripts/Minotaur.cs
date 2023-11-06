using UnityEngine;

public class Minotaur : Enemy
{
    // Start is called before the first frame update
    public MeleeWeapon weapon;
    public float weaponDamage = 10f;
    public float knockBackForce = .4f;
    public GameObject Medkit;

    new void Start()
    {
        base.Start();
        weapon = gameObject.GetComponentInChildren<MeleeWeapon>();
        health = 400;
        maxHealth = 400;
        agent.speed = movementSpeed;

        weapon.damage = weaponDamage;
        weapon.knockBack = knockBackForce;

        pathFindScript.stoppingDistance = 1.5f;
        pathFindScript.shootingDistance = 0;
    }
    public override void DropConsumable()
    {
        Instantiate(Medkit, new Vector3(transform.position.x, Mathf.Max(transform.position.y, 0.5f), transform.position.z), Quaternion.identity);
    }
    public override void Die()
    {
        DropConsumable();
        base.Die();
    }

}