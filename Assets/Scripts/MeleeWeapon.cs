using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public float damage = 10f; // Set the damage amount in the Inspector
    public float knockBack = .4f; // Adjust the force based on your preference

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider entering the trigger is the one you're interested in
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player got attacked");

            PlayerScript player = other.GetComponent<PlayerScript>();

            // If the player has a HealthScript component, apply damage
            if (player != null)
            {
                player.TakeMeleeDamage(damage, knockBack);
            }
        }
    }
}
