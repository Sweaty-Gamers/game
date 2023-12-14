using System.Collections;
using UnityEngine;

class PlayerGrowModifier : Modifier
{
    private GameObject player;
    private PlayerScript play;

    public PlayerGrowModifier(int sec = 10, bool permanent = false)
    {
        this.name = "Player growth";
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
         play = player.GetComponent<PlayerScript>();
         play.big();
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
    }

    protected override IEnumerator end()
    {
        play.normal();
        transform(.5f);
        yield return null;
    }

    protected override IEnumerator permanentMod()
    {
        yield return null;
    }
}