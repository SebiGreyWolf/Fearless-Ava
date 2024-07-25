using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotsMenu : MonoBehaviour
{
    private SaveSlot[] saveSlots;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    private void Start()
    {
        ActivateMenu();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DataPersistanceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if(saveSlot.noDataContent.activeSelf)
        {
            DataPersistanceManager.Instance.NewGame();
        }
        
        SceneManager.LoadSceneAsync(DataPersistanceManager.Instance.GetGameData().level);
    }

    public void OnDeleteSaveClicked(SaveSlot saveSlot)
    {
        DataPersistanceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
        DataPersistanceManager.Instance.DeleteGame();
        ActivateMenu();
    }

    public void ActivateMenu()
    {
        Dictionary<string, GameData> profilesGameData = DataPersistanceManager.Instance.GetAllProfilesGameData();

        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);

        }
    }
}
