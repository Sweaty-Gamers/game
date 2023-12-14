using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEasterEgg : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("EASTER EGG");

            GameObject gameObject = GameObject.Find("GameMaster");
            CustomMasterScript script = gameObject.GetComponent<CustomMasterScript>();
            script.GotEasterEgg();
            Destroy(this);
        }
    }
}
