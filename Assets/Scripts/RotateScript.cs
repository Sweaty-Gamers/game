using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public Vector3 RotateAmount;

    private void Update()
    {
        transform.Rotate(RotateAmount * Time.deltaTime);
    }
}
