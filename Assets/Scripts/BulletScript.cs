using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject bulletMarkPrefab;
    private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despawn(gameObject, 5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Despawn(GameObject gameObject, int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        if (!hit && 
            collision.gameObject.tag != "BulletMark" &&
            collision.gameObject.tag != "Bullet" &&
            collision.gameObject.tag != "Player"
        ) {
            foreach (ContactPoint contact in collision.contacts) {
                GameObject bulletMark = Instantiate(bulletMarkPrefab, contact.point + contact.normal* 0.001f, Quaternion.LookRotation(contact.normal, Vector3.up) * bulletMarkPrefab.transform.rotation);
                StartCoroutine(Despawn(bulletMark, 30));
            }
            hit = true;
        }
    }
}
