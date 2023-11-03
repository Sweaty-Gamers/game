using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Only grows enemies that have already been spawned, no new enemies
class EnemyGrowth : Modifier
{
    public Enemy[] enemies;

    public EnemyGrowth(int sec = 10)
    {
        new WaitForSeconds(10);
        this.sec = sec;
        enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
    }

    protected override IEnumerator end()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.transform.localScale /= 3f;
        }

        yield return null;
    }

    protected override IEnumerator start()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.transform.localScale *= 3f;
        }

        yield return new WaitForSeconds(sec);

        yield return null;
    }
}
