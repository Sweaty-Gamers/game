using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitScript : Consumable
{
    public override void ApplyEffect()
    {
        player.heal(25f);
    }
}