using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> allQuests = new List<Quest>();
    public DialogueManager dialogueManager;
    private Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();

        if (inventory != null)
        {
            inventory.OnInventoryChanged += UpdateQuest;
        }

        foreach (Quest quest in allQuests)
        {
            quest.ResetQuestState();
        }
    }

    public void ActivateQuest(Quest quest)
    {
        quest.isActive = true;
        dialogueManager.UpdateQuestUI(quest);
    }

    void UpdateQuest()
    {
        foreach (Quest quest in allQuests)
        {
            UpdateQuestStatuses(quest);
        }
    }

    public void ActivateReward(Quest quest)
    {
        if (quest.reward != null)
        {
            quest.reward.SetActive(true);
        }
    }
    public void RemoveQuest(Quest quest)
    {
        foreach (var item in quest.requiredItems)
        {
            inventory.RemoveItem(item);
        }
        allQuests.Remove(quest);
    }
    public void UpdateQuestStatuses(Quest quest)
    {
        if (quest.isActive && !quest.isCompleted)
        {
            bool questCompleted = true;
            foreach (Item item in quest.requiredItems)
            {
                if (!inventory.HasItem(item.name, item.maxCount))
                {
                    questCompleted = false;
                    break;
                }
            }

            quest.isCompleted = questCompleted;
            CheckQuestCompletion(quest);
            dialogueManager.UpdateQuestUI(quest);
        }
    }

    public void CheckQuestCompletion(Quest quest)
    {
        foreach (Item item in quest.requiredItems)
        {
            if (!inventory.HasItem(item.name, item.maxCount))
            {
                quest.isCompleted = false;
                return;
            }
        }
        quest.isCompleted = true;
        ActivateReward(quest);
    }
}
