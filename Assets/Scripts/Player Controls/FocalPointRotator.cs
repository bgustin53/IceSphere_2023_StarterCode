using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocalPointRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        //Sets Keyboard input A-D-Left-RIght to [0. 1) to rotate the foal points which the camera is attached to
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }
}
