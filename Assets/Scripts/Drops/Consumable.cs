using System.Collections;
using UnityEngine;

public abstract class Consumable : MonoBehaviour
{
    public Vector3 rotate;
    public float speed;
    public float amp;
    public float originalPosition;
    public PlayerScript player;
    public int despawnTime;
    // Start is called before the first frame update
    public void Start()
    {
        despawnTime = 20;
        speed = 2.0f;
        amp = .25f;
        rotate = new Vector3(0, 25, 0); 
        originalPosition = transform.position.y;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerScript>();
        StartCoroutine(Despawn());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime * rotate);
        transform.position = new Vector3 (transform.position.x, Mathf.Sin(Time.time)*amp + originalPosition, transform.position.z);
    }

    // Has to destroy gameObject once finished.
    public abstract IEnumerator ApplyEffect();


    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ApplyEffect());
            Destroy(gameObject);
        }
    }
}
