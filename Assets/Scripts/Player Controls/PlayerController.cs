using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;
    private SphereCollider playerCollider;
    private Light powerUpIndicator;
    private Transform focalpoint;
    private float moveForce;
    public bool hasPowerUp { get; private set; }

    // OnEnable is called when the player is enabled
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        playerRB = GetComponent<Rigidbody>();
        playerCollider = GetComponent<SphereCollider>();
        powerUpIndicator = GetComponent<Light>();
        playerCollider.material.bounciness = 0.4f;
        powerUpIndicator.intensity = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        if(transform.position.y < -10)
        {
            GameManager.Instance.gameOver = true;
            Debug.Log("You Lost");                 //*****   More will go here after Prototype  ***** //
            gameObject.SetActive(false);
        }
    }

    private void AssignLevelValues()
    { 
        transform.localScale = GameManager.Instance.playerScale;
        playerRB.mass = GameManager.Instance.playerMass;
        playerRB.drag = GameManager.Instance.playerDrag;
        moveForce = GameManager.Instance.playerMoveForce;
        focalpoint = GameObject.Find("Focal Point").transform;
        gameObject.layer = LayerMask.NameToLayer("Player");
        if (GameManager.Instance.debugPowerUpRepel)
        {
            hasPowerUp = true;
        }
    }

    private void Move()
    {
        if (focalpoint != null)
        {
            float verticalInput = Input.GetAxis("Vertical");
            playerRB.AddForce(focalpoint.forward.normalized * verticalInput * moveForce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Startup"))
        {
            collision.gameObject.tag = "Ground";
            playerCollider.material.bounciness = GameManager.Instance.playerBounce;
            AssignLevelValues();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Portal"))
        {
            gameObject.layer = LayerMask.NameToLayer("Portal");
        }

        if(other.gameObject.CompareTag("PowerUp"))
        {
            PowerUpController powerUpController = other.gameObject.GetComponent<PowerUpController>();
            other.gameObject.SetActive(false);
            StartCoroutine(Cooldown(powerUpController.GetCooldown()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Portal"))
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            if(transform.position.y < other.transform.position.y - 1)
            {
                transform.position = Vector3.up * 25;
                GameManager.Instance.switchLevel = true;
            }
        }
    }

    IEnumerator Cooldown(float cooldown)
    {
        hasPowerUp = true;
        powerUpIndicator.intensity = 3.5f;
        yield return new WaitForSeconds(cooldown);
        hasPowerUp = false;
        powerUpIndicator.intensity = 0.0f;
    }
}
