using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPersistance : MonoBehaviour, IDataPersistance
{
    [SerializeField] private string id;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private bool isCompleted = false;


    public void LoadData(GameData data)
    {
        data.tasksCompleted.TryGetValue(id, out isCompleted);
        if(isCompleted)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if(data.tasksCompleted.ContainsKey(id))
        {
            data.tasksCompleted.Remove(id);
        }
        data.tasksCompleted.Add(id, isCompleted);
    }

    private void OnDestroy()
    {
        isCompleted = true;
    }
}
