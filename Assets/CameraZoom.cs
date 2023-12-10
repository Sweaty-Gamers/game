using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 0.2f;
    public float minFOV;
    private float defaultFOV;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        defaultFOV = cam.fieldOfView;
        minFOV = defaultFOV / 1.5f;
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) // Right mouse button is down
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, minFOV, zoomSpeed * Time.deltaTime);
        }
        else // Right mouse button is up
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, zoomSpeed * Time.deltaTime);
        }
    }
}
