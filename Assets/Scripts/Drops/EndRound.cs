using System.Collections;
using UnityEngine;

public class EndRound : Consumable
{
    public override IEnumerator ApplyEffect()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            Enemy e = enemy.GetComponent<Enemy>();
            e.Die();
        }

        yield return null;
    }
}
