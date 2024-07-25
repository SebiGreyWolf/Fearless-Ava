using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPersistance : MonoBehaviour, IDataPersistance
{
    [SerializeField] private string id;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private bool isKilled = false;


    public void LoadData(GameData data)
    {
        data.enemiesKilled.TryGetValue(id, out isKilled);
        if(isKilled)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if(data.enemiesKilled.ContainsKey(id))
        {
            data.enemiesKilled.Remove(id);
        }
        data.enemiesKilled.Add(id, isKilled);
    }

    private void OnDestroy()
    {
        isKilled = true;
    }
}
