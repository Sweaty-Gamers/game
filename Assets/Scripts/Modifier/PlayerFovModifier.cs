using System.Collections;
using UnityEngine;

class PlayerFovModifier : Modifier
{
    public PlayerFovModifier(int sec = 30) {
        this.name = "FOV";
    }

    protected override IEnumerator start()
    {
        float initialFov = Camera.main.fieldOfView;

        float t = 0;
        while (t < 1) {
            float val = Mathf.Lerp(initialFov, 120, t);
            Camera.main.fieldOfView = val;
            t += 0.001f;
            yield return new WaitForSeconds(0.01f);
        }

        // Work for 10s...
        yield return new WaitForSeconds(30);

        t = 0;
        while (t < 1) {
            float val = Mathf.Lerp(120, initialFov, t);
            Camera.main.fieldOfView = val;
            t += 0.001f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }

    protected override IEnumerator end()
    {
        yield return null;
    }

    protected override IEnumerator permanentMod()
    {
        throw new System.NotImplementedException();
    }
}