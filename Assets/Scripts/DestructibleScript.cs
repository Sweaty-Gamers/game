using UnityEngine;

public class DestructibleScript : MonoBehaviour
{
    public float health;
    public float maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    private void TakeDamage(float damage)
    {
        SetNewHealth(health - damage);
        CheckHealth();
    }

    private void SetNewHealth(float newHealth)
    {
        if (newHealth < 0f)
        {
            newHealth = 0f;
        }

        if (newHealth > maxHealth)
        {
            newHealth = maxHealth;
        }

        health = newHealth;
    }

    private void CheckHealth()
    {
        if (health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {

        Debug.Log("reeee");

        if (collision.gameObject.tag == "Bullet")
        {
            BulletScript bullet = collision.gameObject.GetComponent<BulletScript>();
            TakeDamage(bullet.damage);
        }
    }
}
