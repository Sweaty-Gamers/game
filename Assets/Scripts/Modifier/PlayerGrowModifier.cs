using System.Collections;
using UnityEngine;

class PlayerGrowModifier : Modifier
{
    private GameObject player;

    public PlayerGrowModifier(int sec = 10, bool permanent = false) {
        this.sec = sec;
        this.permanent = permanent;
        player = GameObject.Find("Player");
    }

    private void transform(float scale)
    {
        player.transform.localScale *= scale;
    }

    protected override IEnumerator start()
    {

        transform(2f);

        if (permanent)
        {
            yield return permanentMod();
        }
        else
        {
            yield return new WaitForSeconds(sec);
            yield return end();
        }

        player.transform.localScale *= 2f;

        // Work for 10s...
        yield return new WaitForSeconds(10);

        yield return null;
    }

    protected override IEnumerator end()
    {
        transform(.5f);
        yield return null;
    }

    protected override IEnumerator permanentMod()
    {
        yield return null;
    }
}