using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool OptionsMenuIsActive = false;

    public GameObject pauseMenuUI;
    public GameObject OptionsMenuUI;
    public GameObject DialogueUI;
    public Text SaveButtonText;

    public GameObject HealthBar;
    public GameObject FireAbility;
    public GameObject ShieldAbility;
    public GameObject IceAbility;
    public GameObject QuestList;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !DialogueUI.activeSelf)
        {
            if (OptionsMenuIsActive)
            {
                ExitOptionsMenu();
                return;
            }

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

    public void toggleUIElements(bool toggle)
    {
        HealthBar.SetActive(toggle);
        FireAbility.SetActive(toggle);
        ShieldAbility.SetActive(toggle);
        IceAbility.SetActive(toggle);
        QuestList.SetActive(toggle);
    }

    public void OptionsButtonMenu()
    {
        OptionsMenuIsActive = true;
        pauseMenuUI.SetActive(false);
        OptionsMenuUI.SetActive(true);
    }

    public void ExitOptionsMenu()
    {
        OptionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        OptionsMenuIsActive = false;
    }

    public void SaveGameButton()
    {
        DataPersistanceManager.Instance.SaveGame();

        StartCoroutine(GameSaved());
    }

    IEnumerator GameSaved()
    {
        SaveButtonText.text = "Game Saved";
        yield return new WaitForSecondsRealtime(1);
        SaveButtonText.text = "Save";
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
