using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

class TreeGrowModifier : Modifier
{
    private readonly List<GameObject> trees = new();

    public TreeGrowModifier() {
        this.name = "Tree grow";
        trees = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.ToLower().Contains("christmas tree")).ToList();
    }

    protected override IEnumerator start()
    {
        float t = 0;
        while (t < 1) {
            float val = Mathf.Lerp(1, 2, t);
            foreach (var tree in trees)
            {
                tree.transform.localScale = Vector3.one * val;
            }
            
            t += 0.001f;
            yield return new WaitForSeconds(0.01f);
        }

        // Work for 10s...
        yield return new WaitForSeconds(30);

        t = 0;
        while (t < 1) {
            float val = Mathf.Lerp(2, 1, t);
            foreach (var tree in trees)
            {
                tree.transform.localScale = Vector3.one * val;
            }
            
            t += 0.001f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }

    protected override IEnumerator end()
    {
        yield return null;
    }
}