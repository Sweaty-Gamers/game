using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

class SunColorModifier : Modifier
{
    private Light sun;

    public SunColorModifier() {
        this.name = "Sun color";
        sun = GameObject.Find("Sun").GetComponent<Light>();
    }

    private Color ShiftHueBy(Color color, float amount)
    {
        // convert from RGB to HSV
        Color.RGBToHSV(color, out float hue, out float sat, out float val);
 
        // shift hue by amount
        hue += amount;
        sat = 1;
        val = 1;
 
        // convert back to RGB and return the color
        return Color.HSVToRGB(hue, sat, val);
    }

    protected override IEnumerator start()
    {
        Color initialColor = sun.GetComponent<Light>().color;

        float t = 0;
        while (t < 1) {
            float val = Mathf.Lerp(0, 1, t);
            Color newColor = ShiftHueBy(sun.color, 0.001f);
            sun.color = newColor;
            
            t += 0.001f;
            yield return new WaitForSeconds(0.01f);
        }

        // Work for 10s...
        yield return new WaitForSeconds(30);

        sun.color = initialColor;

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