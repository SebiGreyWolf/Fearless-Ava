using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Inventory : MonoBehaviour, IDataPersistance
{
    public static Inventory instance;
    public List<Item> items;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            items = new List<Item>();
            if (QuestManager.instance != null)
                QuestManager.instance.UpdateQuestUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item itemToAdd)
    {
        bool itemAlreadyContained = false;
        foreach (Item item in items)
        {
            if (item.itemName == itemToAdd.itemName)
            {
                itemAlreadyContained = true;
            }
        }

        if (!itemAlreadyContained)
        {
            items.Add(itemToAdd);
        }

        foreach (Item item in items)
        {
            if (item.itemName == itemToAdd.itemName && item.currentCount < item.maxCount)
            {
                item.currentCount++;


                // Notify QuestManager that an item has been added or updated
                QuestManager.instance.CheckQuestsCompletion(items);
                QuestManager.instance.UpdateQuestUI();
            }
            else
            {
                Debug.Log("Max count reached for " + item.itemName);
            }
        }
    }

    public int GetItemCount(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                return item.currentCount;
            }
        }
        return 0; // If item is not found, return 0
    }

    public void LoadData(GameData data)
    {
        foreach (var item in data.itemsToSave)
        {
            items.Add(item);
        }
        QuestManager.instance.UpdateQuestUI();
    }

    public void SaveData(ref GameData data)
    {
        data.itemsToSave.Clear();
        foreach (Item item in items)
        {
            data.itemsToSave.Add(item);
        }
    }
}
