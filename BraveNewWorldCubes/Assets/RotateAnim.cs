using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnim : MonoBehaviour
{
    public float rotateSpeed = 1.0f;
    public Vector3 rotateVector = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(this.transform.position, rotateVector, rotateSpeed * Time.deltaTime);
    }
}
