using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScenes : MonoBehaviour
{
    /*
    [SerializeField] private Button loadGameButton;
    
    private void Start()
    {
        if (!DataPersistanceManager.Instance.hasGameData())
        {
            loadGameButton.interactable = false;
        }
    }
    */

    public void GoToScenePlay()
    {
        //SceneManager.LoadScene("Test");
        //SceneManager.LoadScene("Level1");
        SceneManager.LoadScene("SaveSlotMenu");
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

    /*
    public void OnNewGameClicked()
    {
        DataPersistanceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync("Level1");
    }

    public void OnLoadGame() 
    {
        SceneManager.LoadSceneAsync("Level1");
    }
    */

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}