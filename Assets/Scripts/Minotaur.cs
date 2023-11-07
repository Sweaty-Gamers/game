using System;

public class Minotaur : Enemy
{
    // Start is called before the first frame update
    public MeleeWeapon weapon;
    public static float newHealth;
    public float weaponDamage = 10f;
    public float knockBackForce = .4f;
    public static float healthCap = 500f;

    void Start()
    {
        weapon = gameObject.GetComponentInChildren<MeleeWeapon>();
        maxHealth = MathF.Min(healthCap, newHealth);
        health = maxHealth;
        agent.speed = movementSpeed;
        weapon.damage = weaponDamage;
        weapon.knockBack = knockBackForce;

        pathFindScript.stoppingDistance = 1.5f;
        pathFindScript.shootingDistance = 0;
    }
}