using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileAreasScript : MonoBehaviour
{
    public HealthBar healthbar;
    public Player player;

    private void Start()
    {
        healthbar = GetComponentInChildren<HealthBar>();
    }

    void Update()
    {
        if (player.transform.position.x < -270 || player.transform.position.y < -85)
        {
            healthbar.gameObject.SetActive(true);
        }
        else
        {
            healthbar.gameObject.SetActive(false);
            healthbar.SetHealth(100);
        }
    }
}
