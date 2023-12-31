using System;

public class DragonScript : Enemy
{
    public static float newHealth = 700f;
    public static float healthCap = 1000f;
    public MeleeWeapon fire;
    public float damage = 10f;
    public float knockBack = .5f;

    // Start is called before the first frame update
    new void Start()
    {

        base.Start();

        fire = gameObject.GetComponentInChildren<MeleeWeapon>();
        maxHealth = MathF.Min(healthCap, newHealth);
        health = maxHealth;
        agent.speed = movementSpeed; 
        dropRate = 1f;
        fire.damage = damage;
        fire.knockBack = knockBack;
    }
}
