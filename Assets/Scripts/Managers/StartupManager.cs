using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    private PlayerController[] players;

    private void Start()
    {
        players = FindObjectsOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        SelectPlayerAndStartGame();
    }

    private void SelectPlayerAndStartGame()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits an object
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the object has a player selection component
                PlayerController playerSelected = hit.collider.GetComponent<PlayerController>();

                // Check if a chouce was made
                if (playerSelected != null)
                {
                    foreach(PlayerController player in players)
                    {
                        if(!playerSelected.Equals(player))
                        {
                            Destroy(player.gameObject);
                        }
                    }

                    playerSelected.gameObject.transform.position = Vector3.up * 25;
                    SceneManager.LoadScene("Level1");
                }

            }
        }
    }

}
