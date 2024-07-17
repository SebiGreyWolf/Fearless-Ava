using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldowns : MonoBehaviour
{
    public Attack attackScript;
    private Image image;


    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(name == "IceAbility")
        {
            if (!attackScript.CanAttack(attackScript.lastIceAttackTime, attackScript.iceAttackCooldown))
            {
                image.color = Color.grey;
            }
            else
            {
                image.color = Color.white;
            }
        }
        else if(name == "FireAbility")
        {
            if (!attackScript.CanAttack(attackScript.lastFireAttackTime, attackScript.fireAttackCooldown))
            {
                image.color = Color.grey;
            }
            else
            {
                image.color = Color.white;
            }
        }
    }
}
