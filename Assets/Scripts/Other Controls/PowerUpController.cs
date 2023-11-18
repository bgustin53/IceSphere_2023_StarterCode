using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float cooldown;

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    public float GetCooldown()
    {
        return cooldown;
    }
}
