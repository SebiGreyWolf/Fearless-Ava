using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    public string description;
    public bool isCompleted;
    public bool isActive = true;
    public List<Item> requiredItems;

    // Constructor
    public Quest(string name, string desc, List<Item> itemsRequired)
    {
        questName = name;
        description = desc;
        requiredItems = itemsRequired;
        isCompleted = false;
        isActive = true;
    }

    // Method to check if the quest is completed
    public bool CheckCompletion(List<Item> inventory)
    {
        foreach (var requirement in requiredItems)
        {
            if (requirement.currentCount < requirement.maxCount)
            {
                return false;
            }
        }
        isCompleted = true;
        return true;
    }
}
