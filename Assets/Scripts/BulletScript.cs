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

    void OnCollisionEnter(Collision collision)
    {
        // TODO: destroy bullet on impact always
        if (!hit && collision.gameObject.tag == "Floor")
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                GameObject decalObject = Instantiate(bulletMarkPrefab, contact.point + (contact.normal * 0.025f), Quaternion.identity);
                decalObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Destroy(gameObject);
            }
            hit = true;
        }
    }
}
