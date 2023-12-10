using System.Collections;
using UnityEngine;

public class EndRound : Consumable
{
    public override IEnumerator ApplyEffect()
    {
        GameObject[] minotaurs = GameObject.FindGameObjectsWithTag("Enemy_Melee");
        foreach (GameObject enemy in minotaurs)
        {
            Destroy(enemy.gameObject);
        }
        GameObject[] ranged = GameObject.FindGameObjectsWithTag("Enemy_Ranged");
         foreach (GameObject enemy in ranged)
        {
            Destroy(enemy.gameObject);
        }
         GameObject[] dragons = GameObject.FindGameObjectsWithTag("Enemy_Flying");
          foreach (GameObject enemy in dragons)
        {
            Destroy(enemy.gameObject);
        }
        yield return null;
    }
}
