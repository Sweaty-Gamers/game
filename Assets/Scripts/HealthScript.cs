using Unity.VisualScripting;
using UnityEngine;

public class HealthScript : MonoBehaviour {
    /// Health points.
    public float health;

    /// Max health points;
    public float maxHealth;

    void Start() {

    }

    void Update() {

    }

    void TakeDamage(GameObject bullet) {
        // Take damage for bullet damage
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        health -= bulletScript.damage;

        // Cap min health at zero
        if (health < 0) health = 0;
        if (health == 0) Death();
    }

    void Death() {
        // TODO: what happens when you get hit
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Bullet") {
            TakeDamage(collision.gameObject);
        }
    }
}