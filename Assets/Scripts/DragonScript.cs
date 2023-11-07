using System;

public class DragonScript : Enemy
{
    public static float newHealth = 700f;
    public static float healthCap = 1000;
    public MeleeWeapon fire;
    public float damage = 10f;
    public float knockBack = .5f;
    // Start is called before the first frame update
    void Start()
    {
        fire = gameObject.GetComponentInChildren<MeleeWeapon>();
        maxHealth = MathF.Min(healthCap, newHealth);
        agent.speed = movementSpeed; 

        fire.damage = damage;
        fire.knockBack = knockBack;
    }
}
