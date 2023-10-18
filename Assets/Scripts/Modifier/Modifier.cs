using System;
using System.Collections;
using UnityEngine;

abstract class Modifier {    
    protected abstract IEnumerator start();
    protected abstract IEnumerator end();
    
    public IEnumerator apply(MonoBehaviour monoBehaviour) {
        yield return monoBehaviour.StartCoroutine(start());
        yield return monoBehaviour.StartCoroutine(end());
    }
}