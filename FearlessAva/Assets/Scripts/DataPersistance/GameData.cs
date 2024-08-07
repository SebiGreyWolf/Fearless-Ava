using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string level;
    public int health;
    public float[] position;

    public SerializeableDictionary<string, bool> tasksCompleted;
    public SerializeableList<Item> itemsToSave;

    public GameData() 
    {
        level = "Level1";
        health = 100;
        position = new float[] {0, 0, 0};
        tasksCompleted = new SerializeableDictionary<string, bool>();
        itemsToSave = new SerializeableList<Item>();
    }
}
