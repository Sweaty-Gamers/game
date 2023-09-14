using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float jumpForce;

    private new Rigidbody rigidbody;
    private bool isAirborne = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal > 0) {
            rigidbody.AddRelativeForce(new Vector3(movementSpeed, 0, 0));
        } else if (horizontal < 0) {
            rigidbody.AddRelativeForce(new Vector3(-movementSpeed, 0, 0));
        }

        if (vertical > 0) {
            rigidbody.AddRelativeForce(new Vector3(0, 0, movementSpeed));
        } else if (vertical < 0 ) {
            rigidbody.AddRelativeForce(new Vector3(0, 0, -movementSpeed));
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isAirborne) {
            isAirborne = true;
            rigidbody.AddRelativeForce(new Vector3(0, jumpForce, 0));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && isAirborne) {
            isAirborne = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && !isAirborne) {
            isAirborne = true;
        }
    }
}
