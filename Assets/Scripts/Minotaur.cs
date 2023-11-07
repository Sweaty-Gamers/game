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
}