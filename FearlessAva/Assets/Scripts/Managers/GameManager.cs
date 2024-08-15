using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Item> allItems = new List<Item>();
    public List<Quest> allQuests= new List<Quest>();

    //Doing GameManaging n' Stuff just like Screen loading
   void ResetItemsAndQuests()
   {

   }
    private void Start()
    {
        foreach (var item in allItems)
        {
            item.ResetItemState();
        }
        foreach (var quest in allQuests)
        {
            quest.ResetQuestState();
        }
    }
}
