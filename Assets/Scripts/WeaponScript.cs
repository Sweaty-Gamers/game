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
        Vector3 weaponPosition = transform.position;
        Quaternion cameraRotation = Camera.main.transform.rotation;
        GameObject bullet = Instantiate(bulletPrefab, weaponPosition, cameraRotation);
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

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
