using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> allQuests; // List of all quests in the game

    private Inventory inventory; // Reference to the player's inventory
    private DialogueManager dialogueManager; // Reference to the dialogue manager

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        inventory.OnInventoryChanged += UpdateActiveQuests; // Subscribe to inventory changes
    }

    public void ActivateQuest(Quest quest)
    {
        if (quest != null && !quest.isActive)
        {
            quest.isActive = true;
            UpdateQuestUI();
        }
    }

    public void UpdateActiveQuests()
    {
        foreach (var quest in allQuests)
        {
            if (quest.isActive && !quest.isCompleted)
            {
                UpdateQuest(quest);
            }
        }
    }

    public void UpdateQuest(Quest quest)
    {
        bool completed = true;

        foreach (var item in quest.requiredItems)
        {
            if (inventory.GetItemCount(item.itemName) < item.maxCount)
            {
                completed = false;
                break;
            }
        }

        if (completed)
        {
            Debug.Log("Is comlpeted");
            CompleteQuest(quest);
        }

        UpdateQuestUI();
    }

    public void CompleteQuest(Quest quest)
    {
        if (quest.isActive && !quest.isCompleted)
        {
            quest.isCompleted = true;
            ActivateReward(quest);
            UpdateQuestUI();
        }
    }

    public void ActivateReward(Quest quest)
    {
        // Locate the DialogueTrigger associated with this quest
        DialogueTrigger[] triggers = FindObjectsOfType<DialogueTrigger>();
        foreach (var trigger in triggers)
        {
            
            var relevantDialogue = trigger.questDialogues.Find(q => q.currentQuest == quest);
            if (relevantDialogue != null)
            {
                trigger.HandleReward();
                break;
            }
        }
    }

    public void RemoveQuest(Quest quest)
    {
        if (quest.isActive)
        {
            quest.isActive = false;
            allQuests.Remove(quest);
            FindObjectOfType<AudioManagement>().PlaySound("QuestReward");
            UpdateQuestUI();
        }
    }

    public bool ContainsQuest(Quest quest)
    {
        return allQuests.Contains(quest);
    }

    public bool CheckQuestCompletion(Quest quest)
    {
        return quest.isCompleted;
    }

    public bool CanPickup(Quest quest)
    {
        return quest.requirement == null || quest.requirement.isCompleted;
    }

    private void UpdateQuestUI()
    {
        // Notify the DialogueManager or UI system to update the quest display
        dialogueManager.UpdateQuestUI();
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
