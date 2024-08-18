using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileAreasScript : MonoBehaviour
{
    public GameObject healthbar;
    public Player player;


    void Update()
    {
        if(player.transform.position.x < -270 || player.transform.position.y < -85)
        {
            healthbar.SetActive(true);
        }
        else
        {
            healthbar.SetActive(false);
        }
    }
}
