using Unity.VisualScripting;
using TMPro;

using UnityEngine;

public class HealthScript : MonoBehaviour {
    /// Health points.
    public float health;

    /// Max health points;
    public float maxHealth;

    private GameObject healthUi;
    private TextMeshProUGUI healthText;

    void Start() {
        healthUi = GameObject.Find("Health");
        healthText = healthUi.GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        if (gameObject.tag == "Player") {
            float healthPercent = (int) (health / maxHealth * 100f);
            healthText.text = healthPercent.ToString() + "%";
        }
    }

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
        // TODO: what happens when you get hit
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