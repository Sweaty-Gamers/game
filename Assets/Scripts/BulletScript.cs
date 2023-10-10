using System.Collections;
using System.Linq;
using UnityEngine;

/// Bullet functionality.
public class BulletScript : MonoBehaviour
{
    public GameObject bulletMarkPrefab;
    public int bulletDespawnTime;
    public float damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Disable bullet collisions with the player.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), player.GetComponent<Collider>());

        // Despawn after a certain amount of time.
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
