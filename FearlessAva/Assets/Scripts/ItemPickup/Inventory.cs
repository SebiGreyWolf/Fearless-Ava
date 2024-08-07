using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Inventory : MonoBehaviour, IDataPersistance
{
    public static Inventory instance;
    public SerializeableList<Item> items;
    public Text inventoryText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            items = new SerializeableList<Item>();
            UpdateInventoryUI();
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
            if(item.itemName == itemToAdd.itemName)
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
            if(item.itemName == itemToAdd.itemName && item.currentCount < item.maxCount)
            {
                item.currentCount++;
                UpdateInventoryUI();
            }
            else
            {
                Debug.Log("Max count reached for " + item.itemName);
            }
        }
    }

    void UpdateInventoryUI()
    {
        inventoryText.text = "";
        foreach (var item in items)
        {
            inventoryText.text += $"{item.itemName}: {item.currentCount}/{item.maxCount}\n";
        }
    }

    public void LoadData(GameData data)
    {
        foreach (var item in data.itemsToSave)
        {
            items.Add(item);
        }
        UpdateInventoryUI();
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
