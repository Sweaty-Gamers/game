using UnityEngine;

public class Consumable : MonoBehaviour
{
    Vector3 rotate;
    public float speed;
    public float amp;
    private float originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        rotate = new Vector3(0, 25, 0); 
        originalPosition = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(rotate * speed * Time.deltaTime);
        transform.position = new Vector3 (this.transform.position.x, Mathf.Sin(Time.time)*amp + originalPosition, this.transform.position.z);
    }


}
