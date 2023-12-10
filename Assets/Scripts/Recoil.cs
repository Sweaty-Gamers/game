using UnityEngine;

public class Recoil : MonoBehaviour
{

    //public GameObject Weapon;
    public float maxRecoil_x = -20.0f;
    public float maxRecoil_y = -10.0f;

    public float maxTrans_x = 1.0f;
    public float maxTrans_z = -1.0f;

    public float recoilSpeed = 10.0f;
    public float recoil = 0.0f;

    void Update()
    {
        float _maxRecoil_x = maxRecoil_x;
        float _maxRecoil_y = maxRecoil_y;
        float _maxTrans_x = maxTrans_x;
        float _maxTrans_z = maxTrans_z;

        if (Input.GetMouseButton(1))
        {
            _maxRecoil_x /= 2;
            _maxRecoil_y /= 2;
            _maxTrans_x /= 2;
            _maxTrans_z /= 2;
        }

        if (recoil > 0)
        {
            var maxRecoil = Quaternion.Euler(
                Random.Range(transform.localRotation.x, _maxRecoil_x),
                Random.Range(transform.localRotation.y, _maxRecoil_y),
                transform.localRotation.z);

            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);

            var maxTranslation = new Vector3(
                Random.Range(transform.localPosition.x, _maxTrans_x),
                transform.localPosition.y,
                Random.Range(transform.localPosition.z, _maxTrans_z));

            transform.localPosition = Vector3.Slerp(transform.localPosition, maxTranslation, Time.deltaTime * recoilSpeed);

            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0;
            var minRecoil = Quaternion.Euler(
                Random.Range(0, transform.localRotation.x),
                Random.Range(0, transform.localRotation.y),
                transform.localRotation.z);

            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, minRecoil, Time.deltaTime * recoilSpeed / 2);

            var minTranslation = new Vector3(
                Random.Range(0, transform.localPosition.x),
                transform.localPosition.y,
                Random.Range(0, transform.localPosition.z));

            transform.localPosition = Vector3.Slerp(transform.localPosition, minTranslation, Time.deltaTime * recoilSpeed);
        }
    }
}
