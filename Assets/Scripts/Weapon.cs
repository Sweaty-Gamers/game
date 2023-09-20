using UnityEngine;

public struct Weapon {
    public float fireRate;
    public float reloadTime;
    public int magSize;

    public GameObject weaponModel;
    public GameObject projectileModel;

    public Weapon(
        float fireRate,
        float reloadTime,
        int magSize,
        GameObject weaponModel,
        GameObject projectileModel
    ) {
        this.fireRate = fireRate;
        this.reloadTime = reloadTime;
        this.magSize = magSize;
        this.weaponModel = weaponModel;
        this.projectileModel = projectileModel;
    }

    public static Weapon pistol = new Weapon(
        1f,
        3f,
        12,
        null,   // TODO
        null    // TODO
    );
}