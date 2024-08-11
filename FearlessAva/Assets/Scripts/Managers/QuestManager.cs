using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour, IDataPersistance
{
    public static QuestManager instance;
    public SerializeableList<Quest> quests;
    public Text questUI; // Reference to the UI element to display active quests

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            quests = new SerializeableList<Quest>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool HasQuest(Quest questToAdd)
    {
        return quests.Contains(questToAdd);
    }
    // Add a new quest to the quest list and update the UI
    public void AddQuest(Quest newQuest)
    {
        if (quests.Contains(newQuest))
            return;

        quests.Add(newQuest);
        UpdateQuestUI();
    }


    public void CheckQuestsCompletion(List<Item> inventory)
    {
        foreach (Quest quest in quests)
        {
            if (!quest.isCompleted)
            {
                // Update the current count of required items in the quest based on the inventory
                foreach (var requirement in quest.requiredItems)
                {
                    // Find the corresponding item in the inventory
                    Item inventoryItem = inventory.Find(item => item.itemName == requirement.itemName);

                    if (inventoryItem != null)
                    {
                        // Update the current count in the quest based on the inventory
                        requirement.currentCount = inventoryItem.currentCount;
                    }
                    else
                    {
                        // If the item is not found in the inventory, set the current count to 0
                        requirement.currentCount = 0;
                    }
                }

                // Check if the quest is completed after updating the counts
                if (quest.CheckCompletion(inventory))
                {
                    Debug.Log($"Quest '{quest.questName}' completed!");
                    // Optionally trigger a reward or next step here
                }
            }
        }
    }

    // Update the quest UI
    public void UpdateQuestUI()
    {
        questUI.text = "";

        foreach (Quest quest in quests)
        { 
            if (!quest.isCompleted)
            {
                string questProgress = "";
                foreach (var requirement in quest.requiredItems)
                {
                    Debug.Log($"{requirement.itemName}: {requirement.currentCount}/{requirement.maxCount}");
                    questProgress += $"{requirement.itemName}: {requirement.currentCount}/{requirement.maxCount}\n";
                }
                questUI.text += $"{questProgress}\n";
            }
        }
    }

    public void RemoveQuest(Quest currentQuest)
    {
        quests.Remove(currentQuest);
        questUI.text = "";
    }

    public void LoadData(GameData data)
    {
        foreach (var quest in data.questsToSave)
        {
            quests.Add(quest);
        }
        this.UpdateQuestUI();
    }

    public void SaveData(ref GameData data)
    {
        data.questsToSave.Clear();
        foreach (Quest quest in quests)
        {
            data.questsToSave.Add(quest);
        }
    }
}
