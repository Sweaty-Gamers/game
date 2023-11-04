using System;
using System.Collections;
using UnityEngine;

abstract class Modifier {

    public String name;
    public bool permanent;
    public bool forRound;
    public int sec;

    protected abstract IEnumerator start();
    protected abstract IEnumerator end();

    protected abstract IEnumerator permanentMod();
    
    public IEnumerator apply(MonoBehaviour monoBehaviour) {
        yield return monoBehaviour.StartCoroutine(start());
    }

    public IEnumerator permanentApply(MonoBehaviour monoBehaviour)
    {
        yield return monoBehaviour.StartCoroutine(permanentMod());
    }
}