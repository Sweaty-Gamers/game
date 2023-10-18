using Unity.VisualScripting;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour {
    /// Health points.
    public float health;

    /// Max health points;
    public float maxHealth;

    private GameObject healthUi;
    private GameObject healthBarUi;
    private TextMeshProUGUI healthText;
    public Slider healthBar;
    private Rigidbody playerRigidbody;
    public string gameover;
    public Slider playerHealthBar;


    void Start() {
        playerRigidbody = GetComponent<Rigidbody>();
        healthUi = GameObject.Find("Health");
        healthText = healthUi.GetComponent<TextMeshProUGUI>();

        healthUi = GameObject.Find("PlayerHealthBar");
        playerHealthBar = healthUi.GetComponent<Slider>();

        healthBarUi = GameObject.Find("HealthBar");
        healthBar = healthBarUi.GetComponent<Slider>();

        // Health bar max now depends on player
        // TODO ~~ take health from player specifically for UI, Create UI Script which will only take in player health
        // Have less if statements making sure player stats are being shown.
        if (gameObject.tag == "Player")
        {
            healthBar.maxValue = maxHealth;
        }

    }

    void Update() {
        if (gameObject.tag == "Player") {
            healthText.text = health.ToString() + " / " + maxHealth.ToString();
            healthBar.value = health;
            if (playerHealthBar != null)
            {
                playerHealthBar.value = health;
                Debug.Log(playerHealthBar.value);

            }

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

    public void TakeMeleeDamage(float damage)
    {
        // Take damage for bullet damage
        health -= damage;

        print(health);

        // Cap min health at zero
        if (health < 0) health = 0;
        if (health == 0) Death();

        // Apply knockback
        if (playerRigidbody != null)
        {
            Debug.Log("hereeee");
            // Calculate the direction from the player to the object
            Vector3 knockbackDirection = (playerRigidbody.position - transform.position).normalized;

            // Apply the knockback force
            knockbackDirection.x = 10.0f; // You can adjust this value to control the upward force

            playerRigidbody.AddForce(knockbackDirection * AxeScript.knockbackForce, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("cant find rigid body..");
        }

    }

    void Death() {
        // TODO: what happens when you get hit
        if (gameObject.tag == "Player")
        {
            SceneManager.LoadScene(gameover);
        } else
        {
            Destroy(gameObject);
        }
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