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
    public Recoil RecoilObject;

    public float bulletSpawnXOffset = 0.5f;
    public float bulletSpawnYOffset = 0.5f;

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

        Vector3 screenSpaceBulletSpawn = new(bulletSpawnXOffset, bulletSpawnYOffset, 1);
        Vector3 bulletSpawn = Camera.main.ViewportToWorldPoint(screenSpaceBulletSpawn);

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn, Camera.main.transform.rotation);
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            bullet.transform.LookAt(hit.point);
        }

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

        bulletsLeftInMag--;
        UpdateAmmoUi();

        // Recoil.
        RecoilObject.recoil += 0.1f;
    }

    IEnumerator Reload()
    {
        if (bulletsLeftInMag == magSize) yield break;
        if (reserveBullets == 0) yield break;

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
