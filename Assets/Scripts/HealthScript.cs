using UnityEngine;

public class HealthScript : MonoBehaviour {
    /// Health points.
    public float health;

    /// Max health points;
    public float maxHealth;

    void TakeDamage(GameObject bullet) {
        // Take damage for bullet damage
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        health -= bulletScript.damage;

        print(health);

        // Cap min health at zero
        if (health < 0) health = 0;
        if (health == 0) Death();
    }

    void Death() {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        if (gameObject.tag.Contains("Enemy") && collision.gameObject.tag == "Bullet") {
            TakeDamage(collision.gameObject);
        }

        print(gameObject.tag + " -> " + collision.gameObject.tag);
        if (gameObject.tag == "Player" && collision.gameObject.tag == "EnemyBullet") {
            TakeDamage(collision.gameObject);
            print("AWDHAIUDWUADHU AIUHWDAWDI ADIW HUAWDIU");
        }
    }
}