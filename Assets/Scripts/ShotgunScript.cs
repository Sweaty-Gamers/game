using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : WeaponScript
{
    public float ConeSize;
    public int pelletAmount;

    IEnumerator firePellet()
    {
        var xSpread = Random.Range(1f - ConeSize, 1f + ConeSize);
        var ySpread = Random.Range(1f - ConeSize, 1f + ConeSize);

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Camera.main.transform.rotation);
        bullet.transform.localScale = bullet.transform.localScale * .5f;
        var ray = Camera.main.ScreenPointToRay(new Vector3((Screen.width / 2f) * xSpread, (Screen.height / 2f) * ySpread, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            bullet.transform.LookAt(hit.point);
        }

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
        yield return null;
    }

    public override void FireBullet()
    {
        for (int i = 0; i < pelletAmount; i++)
        {
            StartCoroutine(firePellet());
        }

        bulletsLeftInMag--;
    }
}
