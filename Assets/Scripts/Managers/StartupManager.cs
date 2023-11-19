using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerSelection;
    private int selectedIndex;

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
                Selectable playerSelected = hit.collider.GetComponent<Selectable>();

                // Check if a chouce was made
                if (playerSelected != null)
                {
                    //Extract the index of the choice from the name of the object chosen
                    selectedIndex = playerSelected.selectedIndex;
                    StartCoroutine("LoadLevel1Scene");
                }

            }
        }
    }

    // Loads the Level1 scene asyncronously to move player into game/
    IEnumerator LoadLevel1Scene()
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the Player into the newly loaded Scene
        Scene level1 = SceneManager.GetSceneByName("Level1");
        SceneManager.MoveGameObjectToScene(playerSelection[selectedIndex],level1);
        // Unload the player selection Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
