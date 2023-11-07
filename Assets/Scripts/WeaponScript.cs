using System;
using System.Collections;
using TMPro;
using UnityEngine;

/// Used for weapon functionality.
public class WeaponScript : MonoBehaviour
{
    /// How many shots-per-second.
    public float fireRate;
    /// How many seconds to reload.
    public float reloadTime;
    /// The size of each mag.
    public int magSize;
    /// The speed of the bullet projectile.
    public float bulletSpeed;
    /// Whether or not the player can hold down the trigger.
    public bool semiAuto;
    /// The prefab for the bullet.
    public GameObject bulletPrefab;
    /// The empty parent object to the player to rotate the camera's offsets.
    public Recoil RecoilObject;

    /// The length of the reload animation.
    public float reloadAnimationLength;
    /// The length of the fire animation.
    public float fireAnimationLength;

    /// How many bullets come with the weapon as backup ammo.
    public float reserveBullets;
    /// How many bullets are currently in the mag.
    public float bulletsLeftInMag = 0;

    private bool isReloading = false;
    private float timestampLastBulletFired = -1;

    private Animator animator;
    private PlayerScript playerScript;
    private Transform bulletSpawn;

    // Start is called before the first frame update
    void Start()
    {
        bulletsLeftInMag = magSize;
        animator = GetComponent<Animator>();
        playerScript = GetComponentInParent<PlayerScript>();
        bulletSpawn = transform.Find("Spawn");
    }

    void OnDisable()
    {
        isReloading = false;
    }

    void Shoot()
    {
        if (isReloading) return;

        float curTime = Time.time;
        if (curTime < timestampLastBulletFired + fireRate) return;
        timestampLastBulletFired = curTime;

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

        FireBullet();
    }

    void FireBullet()
    {
        animator.SetBool("Shoot", true);

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Camera.main.transform.rotation);
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            bullet.transform.LookAt(hit.point);
        }

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

        bulletsLeftInMag--;
        playerScript.hud.updateAmmo();

        RecoilObject.recoil += 0.1f;
    }

    IEnumerator Reload()
    {
        if (bulletsLeftInMag == magSize) yield break;
        if (reserveBullets == 0) yield break;

        isReloading = true;
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

        playerScript.hud.updateAmmo();
    }

    public void addBullets(float bulletAmount)
    {
        reserveBullets += bulletAmount;
        playerScript.hud.updateAmmo();
    }

    // Update is called once per frame
    void Update()
    {
        if ((!semiAuto && Input.GetKey(KeyCode.Mouse0)) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.SetBool("Shoot", false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        animator.SetBool("Walk", playerScript.isWalking);
        animator.SetBool("Run", playerScript.isRunning);
    }
}
