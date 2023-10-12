using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle collision with the player
            // ...
            Debug.Log("Hereee");
        }
        else
        {
            Debug.Log("Hit object's tag: " + collision.gameObject.tag);
        }
    }
}
