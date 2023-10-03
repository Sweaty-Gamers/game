using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject bulletMarkPrefab;
    public int bulletDespawnTime;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), player.GetComponent<Collider>());
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

    void OnCollisionEnter(Collision collision)
    {
        // Put bullet mark at first bullet impact point.
        if (collision.gameObject.tag == "Floor")
        {
            ContactPoint firstContact = collision.contacts.First();
            GameObject decalObject = Instantiate(bulletMarkPrefab, firstContact.point + (firstContact.normal * 0.025f), Quaternion.identity);
            decalObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, firstContact.normal);

            // Destroy bullet.
            Destroy(gameObject);
        }
        // If it hit something (except the player themselves), destroy it w/o a bullet impact point.
        else if (collision.gameObject.tag != "Player")
        {
            // Destroy bullet.
            Destroy(gameObject);
        }
    }
}
