using System.Collections;
using UnityEngine;

public class EndRound : Consumable
{
    public override IEnumerator ApplyEffect()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        yield return null;
    }
}
