using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Consumable
{
    public override IEnumerator ApplyEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject currentWeapon = player.weapons.transform.GetChild(i).gameObject;
            WeaponScript weapon = currentWeapon.GetComponent<WeaponScript>();
            weapon.addBullets(weapon.magSize);
        }

        yield return null;
    }
}
