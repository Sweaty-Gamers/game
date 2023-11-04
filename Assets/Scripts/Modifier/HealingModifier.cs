using System.Collections;
using UnityEngine;

class HealingModifier : Modifier
{

    public GameObject player;
    public PlayerScript playerScript;
    public float healAmount;

    public HealingModifier(float healAmount, int sec, bool permanent = false)
    {
        this.name = "Healing";
        this.sec = sec;
        this.permanent = permanent;
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
        this.healAmount = healAmount;
    }
    protected override IEnumerator end()
    {
        yield return null;
    }

    protected override IEnumerator permanentMod()
    {
        while (true)
        {
            playerScript.heal(healAmount);
            yield return new WaitForSeconds(sec);
        }
    }

    protected override IEnumerator start()
    {

        if (permanent)
        {
            permanentMod();
        }


        for (int i = 0; i < sec >> 1; i++)
        {
            Debug.Log("i: " + i);
            playerScript.heal(healAmount);
            yield return new WaitForSeconds(2);
        }
        yield return null;
    }
}
