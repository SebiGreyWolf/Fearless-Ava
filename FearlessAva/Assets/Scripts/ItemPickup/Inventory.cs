using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<Item> items;
    public Dictionary<string, int> itemCounts;
    public Text inventoryText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            itemCounts = new Dictionary<string, int>();
            UpdateInventoryUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item item)
    {
        if (!itemCounts.ContainsKey(item.itemName))
        {
            itemCounts[item.itemName] = 0;
        }

        if (itemCounts[item.itemName] < item.maxCount)
        {
            itemCounts[item.itemName]++;
            UpdateInventoryUI();
        }
        else
        {
            Debug.Log("Max count reached for " + item.itemName);
        }
    }

    void UpdateInventoryUI()
    {
        inventoryText.text = "";
        foreach (var item in items)
        {
            int count = itemCounts.ContainsKey(item.itemName) ? itemCounts[item.itemName] : 0;
            inventoryText.text += $"{item.itemName}: {count}/{item.maxCount}\n";
        }
    }
}
