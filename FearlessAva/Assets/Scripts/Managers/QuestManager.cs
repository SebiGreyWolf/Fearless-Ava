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
        foreach (var q in allQuests)
        {
            if (q.isActive)
            {
                Debug.Log("ActiveQuest");
                return;
            }
        }
        Debug.Log("Got Quest");
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
            //Instantiate(quest.reward, FindObjectOfType<Player>().transform);
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
    }

    public bool ContainsQuest(Quest quest)
    {
        return allQuests.Contains(quest);
    }
    public bool CanPickup(Item item)
    {
        bool canPickupItem = false;
        foreach (var quest in allQuests)
        {
            if (quest.isActive && !quest.isCompleted)
            {
                canPickupItem = quest.requiredItems.Contains(item);
            }
        }
        return canPickupItem;
    }
}
