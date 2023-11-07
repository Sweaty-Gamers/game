public class Minotaur : Enemy
{
    // Start is called before the first frame update
    public MeleeWeapon weapon;
    public static float health;
    public static float maxhealth;
    public float weaponDamage = 10f;
    public float knockBackForce = .4f;

    void Start()
    {
        weapon = gameObject.GetComponentInChildren<MeleeWeapon>();
        health = 400;
        maxHealth = 600;
        agent.speed = movementSpeed;
        weapon.damage = weaponDamage;
        weapon.knockBack = knockBackForce;

        pathFindScript.stoppingDistance = 1.5f;
        pathFindScript.shootingDistance = 0;
    }
    void Update(){
       health = Minotaur.health;
    }
}