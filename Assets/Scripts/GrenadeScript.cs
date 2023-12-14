using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{

    public int fuse = 5;
    public float damage;
    public float radius;
    public float explosionDamage;
    [SerializeField] private ParticleSystem explosionParticles;
    private ParticleSystem explosionParticlesInstance;
    // Start is called before the first frame update
    void Start()
    {
        // Disable player-fired bullet collisions with the player.
        if (gameObject.tag == "Bullet")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), player.GetComponent<Collider>());
        }

        StartCoroutine(StartFuse());

    }

    IEnumerator StartFuse()
    {
        yield return new WaitForSeconds(fuse);
        Explode();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.ToLower().Contains("enemy"))
        {
            Entity entity = collision.gameObject.GetComponent<Entity>();
            entity.TakeDamage(damage);
            Explode();
        }
    }

    private IEnumerator GetEnemies(Collider c)
    {
        if (c.tag.ToLower().Contains("enemy"))
        {
            Entity entity = c.gameObject.GetComponent<Entity>();
            entity.TakeDamage(damage);
        }

        yield return null;
    }

    private void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius);
        
        foreach (Collider c in hitColliders)
        {
            StartCoroutine(GetEnemies(c));
        }

        explosionParticlesInstance = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        
        Destroy(explosionParticlesInstance);
        explosionParticlesInstance.AddComponent<Explosion>();
        Destroy(gameObject);
    }
}
