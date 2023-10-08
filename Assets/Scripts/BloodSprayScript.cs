using System.Collections;
using UnityEngine;

/// Activate blood effects when hit with a bullet.
public class BloodSprayScript : MonoBehaviour
{

    private new ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DoBloodEffect()
    {
        particleSystem.Play(true);
        yield return new WaitForSeconds(0.2f);
        particleSystem.Stop(true);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            StartCoroutine(DoBloodEffect());
        }
    }
}
