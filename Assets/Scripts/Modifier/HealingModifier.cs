using System.Collections;
using UnityEngine;

class HealingModifier : Modifier
{

    public GameObject player;
    public PlayerScript playerScript;
    public float healAmount;

    public HealingModifier(int sec, float healAmount) 
    {
        this.sec = sec;
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
        this.healAmount = healAmount;
    }
    protected override IEnumerator end()
    {
        yield return null;
    }

    protected override IEnumerator start()
    {
        for (int i = 0; i < sec >> 1; i++)
        {
            Debug.Log("i: " + i);
            playerScript.heal(healAmount);
            yield return new WaitForSeconds(2);
        }
        yield return null;
    }
}
