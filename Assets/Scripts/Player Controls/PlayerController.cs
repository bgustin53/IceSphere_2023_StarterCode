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

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // OnEnable is called when the player is enabled
    void OnEnable()
    {
        playerRB = GetComponent<Rigidbody>();
        playerCollider = GetComponent<SphereCollider>();
        powerUpIndicator = GetComponent<Light>();
        focalpoint = GameObject.Find("Focal Point").transform;
        transform.position = GameManager.Instance.playerStartPos;
        transform.localScale = GameManager.Instance.playerScale;
        playerRB.mass = GameManager.Instance.playerMass;
        playerRB.drag = GameManager.Instance.playerDrag;
        moveForce = GameManager.Instance.playerMoveForce;
        playerCollider.material.bounciness = 0;
        powerUpIndicator.intensity = 0;
        gameObject.layer = LayerMask.NameToLayer("Player");
        if (GameManager.Instance.debugPowerUpRepel)
        {
            hasPowerUp = true;
        }
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

    private void Move()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRB.AddForce(focalpoint.forward.normalized * verticalInput * moveForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Startup"))
        {
            collision.gameObject.tag = "Ground";
            playerCollider.material.bounciness = GameManager.Instance.playerBounce;
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
                gameObject.SetActive(false);
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
