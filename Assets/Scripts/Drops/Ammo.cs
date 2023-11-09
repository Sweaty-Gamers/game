using System.Collections;
using UnityEngine;

public class Ammo : Consumable
{

    new private void Start()
    {
        speed = 2.0f;
        amp = .25f;
        rotate = new Vector3(0, 25, 0);
        originalPosition = transform.position.y;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerScript>();
    }
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
