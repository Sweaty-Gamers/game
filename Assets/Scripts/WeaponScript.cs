using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class WeaponScript : MonoBehaviour
{
    public float fireRate;
    public float reloadTime;
    public int magSize;
    public float bulletSpeed;
    public bool semiAuto;
    public GameObject bulletPrefab;
    public GameObject ammoUi;

    public float reloadAnimationLength;
    public float fireAnimationLength;

    public float reserveBullets;
    private float bulletsLeftInMag = 0;
    private bool isReloading = false;
    private float timestampLastBulletFired = -1;

    private TextMeshProUGUI ammoText;
    private Animator animator;
    private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        bulletsLeftInMag = magSize;
        ammoText = ammoUi.GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        playerScript = GetComponentInParent<PlayerScript>();
    }

    void OnEnable()
    {
        UpdateAmmoUi();
    }

    void OnDisable()
    {
        isReloading = false;
        // TODO: Cancel animation
    }

    void UpdateAmmoUi()
    {
        if (ammoText == null) return;
        ammoText.text = bulletsLeftInMag + "/" + reserveBullets;
    }

    void Shoot()
    {
        if (isReloading) return;

        if (bulletsLeftInMag <= 0 && reserveBullets > 0)
        {
            StartCoroutine(Reload());
            return;
        }
        else if (bulletsLeftInMag <= 0)
        {
            // TODO: play *click* sound for empty mag
            return;
        }

        StartCoroutine(FireBullet());
    }

    IEnumerator FireBullet()
    {
        animator.SetFloat("FireSpeed", fireAnimationLength / fireRate);
        animator.SetBool("Shoot", true);

        Vector3 screenSpaceCenter = new(0.5f, 0.5f, 1);
        Vector3 screenCenter = Camera.main.ViewportToWorldPoint(screenSpaceCenter);
        GameObject bullet = Instantiate(bulletPrefab, screenCenter, Camera.main.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * bulletSpeed;
        bulletsLeftInMag--;
        UpdateAmmoUi();

        yield return new WaitForSeconds(fireRate * 0.9f);

        animator.SetBool("Shoot", false);
    }

    IEnumerator Reload()
    {
        if (bulletsLeftInMag == magSize) yield break;

        isReloading = true;
        UpdateAmmoUi();
        animator.SetFloat("ReloadSpeed", reloadAnimationLength / reloadTime);
        animator.SetBool("Reload", true);
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

        animator.SetBool("Reload", false);
        isReloading = false;

        UpdateAmmoUi();
    }

    // Update is called once per frame
    void Update()
    {
        if ((!semiAuto && Input.GetKey(KeyCode.Mouse0)) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            float curTime = Time.time;
            if (curTime > timestampLastBulletFired + fireRate)
            {
                timestampLastBulletFired = curTime;
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        animator.SetBool("Walk", playerScript.isWalking);
        animator.SetBool("Run", playerScript.isRunning);
    }
}
