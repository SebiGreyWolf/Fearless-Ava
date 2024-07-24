using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSaves : MonoBehaviour
{
    SaveData[] saves = new SaveData[3];
    GameObject player;
    Player playerScript;

    void Start()
    {
        SaveSystem.Initialize();
        saves = SaveSystem.LoadAll();
    }

    public void LoadSave1()
    {
        SceneManager.LoadScene("Level" + saves[0].level);
        player = GameObject.FindWithTag("Player");
        Debug.Log(player);
        //playerScript = player.GetComponent<Player>();
        //player.transform.position = new Vector3(saves[0].savePoint[0], saves[0].savePoint[1], saves[0].savePoint[2]);
        //playerScript.currentHealth = saves[0].health;
    }
}
