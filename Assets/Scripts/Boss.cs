using UnityEngine;

public class Boss : Enemy
{
    // Start is called before the first frame update
    public static float newHealth = 1000f;
    //public static float newSpeed;
    public static float healthCap = 1000f;
    //public static float maxSpeed = 4f;
    public float damage = 10f; // Set the damage amount in the Inspector
    public float knockBack = .4f; // Adjust the force based on your preference

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider entering the trigger is the one you're interested in
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player got attacked");

            PlayerScript player = other.GetComponent<PlayerScript>();

            // If the player has a HealthScript component, apply damage
            if (player != null)
            {
                player.TakeMeleeDamage(damage, knockBack);
            }
        }
    }

    new void Start()
    {
        base.Start();
        newHealth = Mathf.Min(healthCap, newHealth);
        //movementSpeed = Mathf.Min(maxSpeed, newSpeed);
        maxHealth = healthCap;
        health = newHealth;
        //agent.speed = movementSpeed;
        dropRate = .2f;
    }

    public float getBossHealth()
    {
        return health;
    }
}