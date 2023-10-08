using System.Collections;
using UnityEngine;

/// Automatically despawn a bullet impact mark.
public class DespawnBulletMarkScript : MonoBehaviour
{
    public int bulletMarkDespawnTime;

    IEnumerator Despawn(GameObject gameObject, int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despawn(gameObject, bulletMarkDespawnTime));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
