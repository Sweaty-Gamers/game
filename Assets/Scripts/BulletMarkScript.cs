using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMarkScript : MonoBehaviour
{
    public int bulletMarkDespawnTime;

    IEnumerator Despawn(GameObject gameObject, int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despawn(gameObject, bulletMarkDespawnTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
