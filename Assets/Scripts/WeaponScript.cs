using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public float fireRate;
    public float reloadTime;
    public int magSize;
    public float bulletSpeed;

    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator DespawnBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(3);
        Destroy(bullet);
    }

    void Shoot() {
        Vector3 screenSpaceCenter = new(0.5f, 0.5f, 1);
        Vector3 screenCenter = Camera.main.ViewportToWorldPoint(screenSpaceCenter);
        GameObject bullet = Instantiate(bulletPrefab, screenCenter, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * bulletSpeed;

        StartCoroutine(DespawnBullet(bullet));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Shoot();
        }
    }
}
