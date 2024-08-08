using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> quests;
    public Text questUI; // Reference to the UI element to display active quests

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            quests = new List<Quest>();
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

    // Check all quests for completion
    public void CheckQuestsCompletion(List<Item> inventory)
    {
        foreach (Quest quest in quests)
        {
            if (!quest.isCompleted && quest.CheckCompletion(inventory))
            {
                Debug.Log($"Quest '{quest.questName}' completed!");
                // Optionally trigger a reward or next step here
            }
        }
        //UpdateQuestUI();
    }

    // Update the quest UI
    public void UpdateQuestUI()
    {
        foreach (Quest quest in quests)
        {
            Debug.Log(quests.Count);
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
}
