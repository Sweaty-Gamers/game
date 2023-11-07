using UnityEngine;

public class Minotaur : Enemy
{
    // Start is called before the first frame update
    public MeleeWeapon weapon;
    public static float newHealth;
    public float weaponDamage = 10f;
    public float knockBackForce = .4f;
    public static float healthCap = 100f;

    new void Start()
    {
        base.Start();
        weapon = gameObject.GetComponentInChildren<MeleeWeapon>();
        newHealth = Mathf.Min(healthCap, newHealth);
        maxHealth = newHealth;
        health = maxHealth;
        agent.speed = movementSpeed;
        dropRate = .5f;

        weapon.damage = weaponDamage;
        weapon.knockBack = knockBackForce;

        pathFindScript.stoppingDistance = 1.5f;
        pathFindScript.shootingDistance = 0;
    }
}