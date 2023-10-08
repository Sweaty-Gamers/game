using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float movementSpeed;
    public float jumpForce;
    public float sprintFactor;
    public float groundDrag;
    public GameObject weapons;
    public float maxSpeed;

    public bool isWalking = false;
    public bool isRunning = false;
    public bool isJumping = false;

    private new Rigidbody rigidbody;
    private int weaponIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

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
    }

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

        // Add drag.
        rigidbody.drag = isJumping ? 1 : groundDrag;

        float speed = movementSpeed;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = isWalking;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        if (isRunning)
        {
            speed *= sprintFactor;
        }

        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            rigidbody.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        if (horizontal > 0)
        {
            rigidbody.AddRelativeForce(new Vector3(speed, 0, 0), ForceMode.Force);
        }

        if (horizontal < 0)
        {
            rigidbody.AddRelativeForce(new Vector3(-speed, 0, 0), ForceMode.Force);
        }

        if (vertical > 0)
        {
            rigidbody.AddRelativeForce(new Vector3(0, 0, speed), ForceMode.Force);
        }

        if (vertical < 0)
        {
            rigidbody.AddRelativeForce(new Vector3(0, 0, -speed), ForceMode.Force);
        }

        // Limit velocity.
        Vector3 velocity = new(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        if (velocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = velocity.normalized * movementSpeed;
            rigidbody.velocity = new(limitedVelocity.x, rigidbody.velocity.y, limitedVelocity.z);
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && isJumping)
        {
            isJumping = false;
        }
    }
}
