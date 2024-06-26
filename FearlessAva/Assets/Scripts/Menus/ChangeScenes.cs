using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public void GoToScenePlay()
    {
        Debug.Log("Play");

        SceneManager.LoadScene("PrototypeScene");
        //SceneManager.LoadScene("Level1");
    }

    public void GoToSceneOptionsMenu()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void GoToSceneMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void GoToSceneCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}