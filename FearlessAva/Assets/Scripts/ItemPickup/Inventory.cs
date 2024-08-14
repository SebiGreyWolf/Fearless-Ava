using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged;

    private void Start()
    {
        foreach (var item in items)
        {
            item.ResetItemState();
        }
    }
    public void AddItem(Item item)
    {
        Item existingItem = items.Find(i => i.name == item.name);
        if (existingItem != null && existingItem.currentCount < existingItem.maxCount)
        {
            existingItem.currentCount += item.currentCount;
        }
        else
        {
            items.Add(item);
            item.currentCount++;
        }
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(Item item)
    {
        Item existingItem = items.Find(i => i.name == item.name);
        if (existingItem != null)
        {
            existingItem.currentCount -= item.currentCount;
            if (existingItem.currentCount <= 0)
            {
                items.Remove(existingItem);
            }
            OnInventoryChanged?.Invoke();
        }
    }

    public bool HasItem(string itemName, int amount)
    {
        Item existingItem = items.Find(i => i.name == itemName);
        return existingItem != null && existingItem.currentCount >= amount;
    }

    public int GetItemCount(string itemName)
    {
        Item existingItem = items.Find(i => i.name == itemName);
        return existingItem != null ? existingItem.currentCount : 0;
    }
}
