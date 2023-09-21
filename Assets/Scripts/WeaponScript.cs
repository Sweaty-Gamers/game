using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public float fireRate;
    public float reloadTime;
    public int magSize;
    public float bulletSpeed;
    public GameObject bulletPrefab;

    public float reserveBullets;
    private float bulletsLeftInMag = 0;
    private bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        bulletsLeftInMag = magSize;
    }

    void Shoot() {
        if (isReloading) return;

        if (bulletsLeftInMag <= 0 && reserveBullets > 0) {
            StartCoroutine(Reload());
            return;
        } else if (bulletsLeftInMag <= 0) {
            // TODO: play *click* sound for empty mag
            return;
        }

        Vector3 screenSpaceCenter = new(0.5f, 0.5f, 1);
        Vector3 screenCenter = Camera.main.ViewportToWorldPoint(screenSpaceCenter);
        GameObject bullet = Instantiate(bulletPrefab, screenCenter, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * bulletSpeed;

        bulletsLeftInMag--;
    }

    IEnumerator Reload()
    {
        if (bulletsLeftInMag == magSize) yield break;

        isReloading = true;
        // TODO: play animation
        yield return new WaitForSeconds(reloadTime);

        /*
            MAG     RESERVE
            3       100
            *reload*
            20      87

            3       10
            *reload*
            13      0
        */

        float bulletsToLoad = Math.Min(
            magSize - bulletsLeftInMag,
            reserveBullets
        );

        reserveBullets -= bulletsToLoad;
        bulletsLeftInMag += bulletsToLoad;

        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Shoot();
        }
    }
}
