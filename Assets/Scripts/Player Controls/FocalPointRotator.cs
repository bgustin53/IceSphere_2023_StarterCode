using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************************************************
 * FocalPointRotator is attached to a Focal Point.  It moves the camera
 * around the island with user controls.
 * 
 * Bruce Gustin
 * November 23, 2023
 ************************************************************************/

public class FocalPointRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;     // How fast the user can rotate around the island

    // Update is called once per frame
    void Update()
    {
        //Sets Keyboard input A-D-Left-RIght to [0. 1) to rotate the foal points which the camera is attached to
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }
}
