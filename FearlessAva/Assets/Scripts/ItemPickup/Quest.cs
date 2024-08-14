using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string questDescription;
    public bool isCompleted = false;
    public bool isActive = false;
    public List<Item> requiredItems;
    public GameObject reward;

    public bool shouldResetOnPlay = true;
    public void ResetQuestState()
    {
        if (shouldResetOnPlay)
        {
            isCompleted = false;
            isActive = false;
            reward.SetActive(false);
        }
    }
}
