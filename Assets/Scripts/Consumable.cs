using UnityEngine;

public abstract class Consumable : MonoBehaviour
{
    Vector3 rotate;
    public float speed;
    public float amp;
    private float originalPosition;
    public PlayerScript player;
    // Start is called before the first frame update
    public void Start()
    {
        speed = 2.0f;
        amp = .25f;
        rotate = new Vector3(0, 25, 0); 
        originalPosition = this.transform.position.y;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(speed * Time.deltaTime * rotate);
        transform.position = new Vector3 (this.transform.position.x, Mathf.Sin(Time.time)*amp + originalPosition, this.transform.position.z);
    }

    public abstract void ApplyEffect();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyEffect();
            Destroy(this.gameObject);
        }
    }
}
