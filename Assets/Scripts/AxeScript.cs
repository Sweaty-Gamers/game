using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider entering the trigger is the one you're interested in
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player got attacked");
        }
    }
}
