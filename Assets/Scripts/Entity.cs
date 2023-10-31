using UnityEngine;

public class Entity : MonoBehaviour
{
    public float health;
    public float maxHealth;


    /// How fast the entity walks.
    public float movementSpeed;

    public virtual void TakeDamage(float damage)
    {
        SetNewHealth(health - damage);
        CheckHealth();
    }

    void heal(float healAmount)
    {
        SetNewHealth(health + healAmount);
        CheckHealth();
    }

    public void SetNewHealth(float newHealth)
    {
        if (newHealth < 0f)
        {
            newHealth = 0;
        }

        if (newHealth > maxHealth)
        {
            newHealth = maxHealth;
        }

        health = newHealth;
    }

    public void CheckHealth()
    {
        if (health < 0f)
        {
            health = 0f;
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health == 0f)
        {
            Die();
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        print("got to collision");
        Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.tag == "Bullet")
        {
            BulletScript bullet = collision.gameObject.GetComponent<BulletScript>();
            TakeDamage(bullet.damage);
        }
    }

    public virtual void Die()
    {
        print("entity committed die");
        Destroy(gameObject);
    }
}