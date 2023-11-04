using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Only grows enemies that have already been spawned, no new enemies
class EnemyGrowth : Modifier
{
    public Enemy[] enemies;

    public EnemyGrowth(int sec = 10, bool permanent = false)
    {
        this.permanent = permanent;
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

    private void transform()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.transform.localScale *= 3f;
        }
    }

    protected override IEnumerator permanentMod()
    {
        yield return null;
    }

    protected override IEnumerator start()
    {

        transform();

        if (permanent)
        {
            yield return permanentMod();
        }
        else
        {
            yield return new WaitForSeconds(sec);
            yield return end();
        }

    }
}
