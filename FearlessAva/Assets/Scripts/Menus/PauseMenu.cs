using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;


    public GameObject pauseMenuUI;

    public GameObject HealthBar;
    public GameObject FireAbility;
    public GameObject ShieldAbility;
    public GameObject IceAbility;


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        toggleUIElements(true);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        toggleUIElements(false);
    }

    private void toggleUIElements(bool toggle)
    {
        HealthBar.SetActive(toggle);
        FireAbility.SetActive(toggle);
        ShieldAbility.SetActive(toggle);
        IceAbility.SetActive(toggle);
    }

    public void SaveGameButton()
    {
        DataPersistanceManager.Instance.SaveGame();
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
