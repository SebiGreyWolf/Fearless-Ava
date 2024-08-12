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

    public bool HasQuest(Quest questToCheck)
    {
        return quests.Exists(q => q.questName == questToCheck.questName && !q.isCompleted);
    }

    public bool HasCompletedQuest(Quest questToCheck)
    {
        return quests.Exists(q => q.questName == questToCheck.questName && q.isCompleted);
    }

    public void AddQuest(Quest newQuest)
    {
        if (!quests.Contains(newQuest))
        {
            quests.Add(newQuest);
            UpdateQuestUI();
        }
    }

    public void CheckQuestsCompletion(List<Item> inventory)
    {
        foreach (Quest quest in quests)
        {
            if (!quest.isCompleted)
            {
                foreach (var requirement in quest.requiredItems)
                {
                    Item inventoryItem = inventory.Find(item => item.itemName == requirement.itemName);

                    if (inventoryItem != null)
                    {
                        requirement.currentCount = inventoryItem.currentCount;
                    }
                    else
                    {
                        requirement.currentCount = 0;
                    }
                }

                if (quest.CheckCompletion(inventory))
                {
                    quest.isActive = false;
                    quest.isCompleted = true; // Mark the quest as completed
                    Debug.Log($"Quest '{quest.questName}' completed!");
                    // Optionally trigger a reward or next step here
                }
            }
        }

        UpdateQuestUI();
    }

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
                questUI.text += $"{quest.questName}:\n{questProgress}\n";
            }
        }
    }

    public void RemoveQuest(Quest currentQuest)
    {
        quests.Remove(currentQuest);
        UpdateQuestUI();
    }

    public void LoadData(GameData data)
    {
        quests = data.questsToSave; // Assuming questsToSave is of type SerializeableList<Quest>
        UpdateQuestUI();
    }

    public void SaveData(ref GameData data)
    {
        data.questsToSave = quests; // Save the current list of quests
    }
}
