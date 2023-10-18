using System.Collections;
using UnityEngine;

class PlayerGrowModifier : Modifier
{
    private GameObject player;

    public PlayerGrowModifier() {
        player = GameObject.Find("Player");
    }

    protected override IEnumerator start()
    {
        player.transform.localScale *= 2f;

        // Work for 10s...
        yield return new WaitForSeconds(10);

        yield return null;
    }

    protected override IEnumerator end()
    {
        player.transform.localScale /= 2f;

        yield return null;
    }
}