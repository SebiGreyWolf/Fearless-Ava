using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    //public Sprite itemIcon;
    public int currentCount;
    public int maxCount;


    public bool shouldResetOnPlay = true;
    public void ResetItemState()
    {
        if (shouldResetOnPlay)
        {
            currentCount = 0;
        }
    }
}
