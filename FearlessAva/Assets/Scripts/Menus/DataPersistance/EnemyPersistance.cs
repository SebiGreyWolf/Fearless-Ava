using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPersistance : MonoBehaviour, IDataPersistance
{
    [SerializeField] string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }


    public void LoadData(GameData data)
    {
        throw new System.NotImplementedException();
    }

    public void SaveData(ref GameData data)
    {
        throw new System.NotImplementedException();
    }

    private void OnDestroy()
    {
        
    }
}
