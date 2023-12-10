using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitScript : Consumable
{
    public override IEnumerator ApplyEffect()
    {
        player.heal(25f);
        Destroy(gameObject);
        yield return null;
    }
}