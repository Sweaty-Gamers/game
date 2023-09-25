using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject bulletMarkPrefab;
    public int bulletDespawnTime;
    private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despawn(gameObject, bulletDespawnTime));
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
                Instantiate(bulletMarkPrefab, contact.point + contact.normal * 0.001f, Quaternion.LookRotation(contact.normal, Vector3.up) * bulletMarkPrefab.transform.rotation);
                Destroy(gameObject);
            }
            hit = true;
        }
    }
}
