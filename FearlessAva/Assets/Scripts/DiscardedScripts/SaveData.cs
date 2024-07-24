using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public string level;
    public int health;
    public float[] savePoint;

    public SaveData(Player player)
    {
        level = player.currentLevel;

        savePoint = new float[3];
        savePoint[0] = player.transform.position.x;
        savePoint[1] = player.transform.position.y;
        savePoint[2] = player.transform.position.z;

        health = player.currentHealth;
    }

    public SaveData()
    {
        level = "Level1";
        health = 20;
        savePoint = new float[] { 0, 0, 0 };
    }
    

}
