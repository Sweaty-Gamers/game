using System.Collections;
using UnityEngine;

class PlayerFovModifier : Modifier
{   
    public float initialFov;
    public PlayerFovModifier(int sec = 30, bool permanent = false)
    {
        this.name = "FOV";
    }

    protected override IEnumerator start()
    {
        initialFov = Camera.main.fieldOfView;

        float t = 0;
        while (t < 1)
        {
            float val = Mathf.Lerp(initialFov, 120, t);

            // If player is scoping in, do not change FOV.
            if(!Input.GetMouseButton(1))
            {
                Camera.main.fieldOfView = val;
            }

            t += 0.001f;
            yield return new WaitForSeconds(0.01f);
        }

        // Work for 30s...
        yield return new WaitForSeconds(10);

        t = 0;

        yield return end();
    }

    protected override IEnumerator end()
    {
        
        Camera.main.fieldOfView = initialFov;
        yield return null;
    }

    protected override IEnumerator permanentMod()
    {
        yield return null;
    }
}