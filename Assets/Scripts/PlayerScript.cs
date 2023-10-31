using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : Entity
{
    /// How fast the player dashes.
    public float dashSpeed;
    /// Jump strength.
    public float jumpForce;
    /// Movement speed multiplier when sprinting.
    public float sprintFactor;
    /// The child empty GameObject that holds the weapons.
    public GameObject weapons;
    /// The max player speed.
    public float maxSpeed;
    /// How quickly sprint fills back up.
    public float sprintRegenerationSpeed;
    /// How quickly sprint drains.
    public float sprintDegenerationSpeed;
    /// How much drag to apply on the floor.
    public float groundDrag;
    /// How much drag to apply on the air.
    public float airDrag;

    public bool iceSkates;

    public bool isWalking = false;
    public bool isRunning = false;
    public bool isJumping = false;
    public bool isDashing = false;
    public float sprintMeter = 100f;
    public float sprintThreshold = 30f;
    public bool recharge = false;

    public int maxJumpsLeft = 2;
    private int jumpsLeft;

    public string deathScreen;
    public HUD hud;
    public Rigidbody rigidBody;
    private int weaponIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        health = 100;

        jumpsLeft = maxJumpsLeft;
        rigidBody = GetComponent<Rigidbody>();

        hud = GetComponent<HUD>();

        for (int i = 0; i < weapons.transform.childCount; i++)
        {
            weapons.transform.GetChild(i).gameObject.SetActive(false);
        }
        weapons.transform.GetChild(weaponIndex).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchWeapons();
        Movement();
        Pause();
    }

    void FixedUpdate()
    {
        Sprint();
    }

    void Sprint()
    {
        if (sprintMeter == 0) recharge = true;

        if (recharge && sprintMeter < sprintThreshold)
        {
            sprintMeter += sprintRegenerationSpeed;
            hud.updateEnergy();
            return;
        }

        recharge = false;

        if (isRunning)
        {
            sprintMeter -= sprintDegenerationSpeed;
        } else
        {
            sprintMeter += sprintRegenerationSpeed;
        }

        sprintMeter = Mathf.Clamp(sprintMeter, 0, 100);
        hud.updateEnergy();
    }

    void Pause()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Time.timeScale = Time.timeScale == 0 ? 1f : 0f;
        }
    }

    /// Switch weapons using the number keys or the mouse wheel.
    void SwitchWeapons()
    {
        int len = weapons.transform.childCount;

        if (Input.GetAxis("Mouse ScrollWheel") > 0) SwitchWeapon(Math.Min(weaponIndex + 1, len - 1));
        if (Input.GetAxis("Mouse ScrollWheel") < 0) SwitchWeapon(Math.Max(weaponIndex - 1, 0));

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchWeapon(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchWeapon(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SwitchWeapon(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SwitchWeapon(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) SwitchWeapon(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) SwitchWeapon(8);
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        isWalking = horizontal != 0 || vertical != 0;

        // Set the player speed depending on if we are sprinting or not.
        float speed = movementSpeed;
        isRunning = Input.GetKey(KeyCode.LeftShift) && isWalking && !recharge;

        // Player can dash if starts moving while holding shift.
        bool wantsToDash = Input.GetKeyDown(KeyCode.Q) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));

        if (isRunning)
            speed *= sprintFactor;

        // Jumping.
        if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0)
        {
            isJumping = true;
            jumpsLeft--;
            rigidBody.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        if (wantsToDash && !isDashing && isJumping)
        {
            // Dashing movement.
            if (horizontal > 0)
            {
                rigidBody.AddForce(Camera.main.transform.forward * dashSpeed, ForceMode.Impulse);
            }

            if (horizontal < 0)
            {
                rigidBody.AddForce(Camera.main.transform.forward * -dashSpeed, ForceMode.Impulse);
            }

            if (vertical > 0)
            {
                rigidBody.AddForce(Camera.main.transform.forward * dashSpeed, ForceMode.Impulse);
            }

            if (vertical < 0)
            {
                rigidBody.AddForce(Camera.main.transform.forward * -dashSpeed, ForceMode.Impulse);
            }

            isDashing = true;
        }

        // Flat movement.
        if (horizontal > 0)
        {
            rigidBody.AddRelativeForce(new Vector3(speed, 0, 0), ForceMode.Force);
        }

        if (horizontal < 0)
        {
            rigidBody.AddRelativeForce(new Vector3(-speed, 0, 0), ForceMode.Force);
        }

        if (vertical > 0)
        {
            rigidBody.AddRelativeForce(new Vector3(0, 0, speed), ForceMode.Force);
        }

        if (vertical < 0)
        {
            rigidBody.AddRelativeForce(new Vector3(0, 0, -speed), ForceMode.Force);
        }

        // Add drag to make movement feel natural.
        rigidBody.drag = isJumping ? airDrag : groundDrag;

        // Limit velocity.
        Vector3 velocity = new(rigidBody.velocity.x, 0, rigidBody.velocity.z);
        if (velocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = velocity.normalized * movementSpeed;
            rigidBody.velocity = new(limitedVelocity.x, rigidBody.velocity.y, limitedVelocity.z);
        }
    }

    void SwitchWeapon(int nextWeaponIndex)
    {
        if (weaponIndex == nextWeaponIndex) return;
        for (int i = 0; i < weapons.transform.childCount; i++)
        {
            weapons.transform.GetChild(i).gameObject.SetActive(false);
        }

        weaponIndex = nextWeaponIndex;
        GameObject currentWeapon = weapons.transform.GetChild(weaponIndex).gameObject;
        currentWeapon.SetActive(true);
    }

    public void damageTaken(float damage)
    {
        TakeDamage(damage);
        hud.updateHealth();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && isJumping)
        {
            isJumping = false;
            isDashing = false;
            jumpsLeft = 2;
        }

        if (collision.gameObject.tag == "EnemyBullet")
        {
            BulletScript bullet = collision.gameObject.GetComponent<BulletScript>();
            damageTaken(bullet.damage);
            print(gameObject.tag + " -> " + collision.gameObject.tag);
        }
    }

    public override void Die()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(deathScreen);
    }

    // Since only player takes melee damage, made unique only to player
    public void TakeMeleeDamage(float damage)
    {
        print("melee stoooof");
        // Applies damage to player
        damageTaken(damage);
        // Applies Knockback
        if (rigidBody != null)
        {
            // Calculate the direction from the player to the object
            Vector3 knockbackDirection = (rigidBody.position - transform.position).normalized;

            // Apply the knockback force upwards
            knockbackDirection.x = 0.25f; // You can adjust this value to control the upward force

            rigidBody.AddForce(knockbackDirection * AxeScript.knockbackForce, ForceMode.Impulse);
        }
    }
}
