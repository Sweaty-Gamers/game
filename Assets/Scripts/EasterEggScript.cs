using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggScript : MonoBehaviour
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
            GameMasterScript script = gameObject.GetComponent<GameMasterScript>();
            script.GotEasterEgg();
            // script.Test();


            Destroy(this);
        }
    }
}
