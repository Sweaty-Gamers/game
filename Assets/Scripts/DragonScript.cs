public class DragonScript : Enemy
{

    public MeleeWeapon fire;
    public float damage = 10f;
    public float knockBack = .5f;

    // Start is called before the first frame update
    void Start()
    {

        base.Start();

        fire = gameObject.GetComponentInChildren<MeleeWeapon>();
        health = 10;
        maxHealth = 10;
        agent.speed = movementSpeed; 

        fire.damage = damage;
        fire.knockBack = knockBack;
    }
}
