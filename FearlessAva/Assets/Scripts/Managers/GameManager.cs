using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Doing GameManaging n' Stuff just like Level loading
    private string scene1Name = "Test";
    private string scene2Name = "Test1";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SwitchScenes();
        }
    }

    void SwitchScenes()
    {
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Determine the next scene to load
        string nextSceneName = currentScene.name == scene1Name ? scene2Name : scene1Name;

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
