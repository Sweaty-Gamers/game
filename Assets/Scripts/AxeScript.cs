using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    public float axeDamage = 10f; // Set the damage amount in the Inspector
    public static float knockbackForce = 50f; // Adjust the force based on your preference
    
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider entering the trigger is the one you're interested in
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player got attacked");

            // Attempt to get the HealthScript component on the player
            HealthScript healthScript = other.GetComponent<HealthScript>();

            // If the player has a HealthScript component, apply damage
            if (healthScript != null)
            {
                healthScript.TakeMeleeDamage(axeDamage);
            }
        }
    }
}
