using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class IncreaseHealthModifier : Modifier
{

    GameObject player;
    PlayerScript playerScript;
    bool percentValue;
    float healthIncreaseAmount;

    public IncreaseHealthModifier(float healthIncreaseAmount, bool percentValue = false)
    {
        name = "IncreaseHealth";
        this.healthIncreaseAmount = healthIncreaseAmount;
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    protected override IEnumerator end()
    {
        yield return null;
    }

    protected override IEnumerator permanentMod()
    {
        yield return null;
    }

    protected override IEnumerator start()
    {
        if (percentValue)
        {
            float newHealth = playerScript.maxHealth * healthIncreaseAmount + playerScript.health;
            playerScript.maxHealth = healthIncreaseAmount * playerScript.maxHealth + playerScript.maxHealth;
            playerScript.health = newHealth;
        } else
        {
            playerScript.maxHealth += healthIncreaseAmount;
            playerScript.health += healthIncreaseAmount;
        }
        yield return null;
    }
}
