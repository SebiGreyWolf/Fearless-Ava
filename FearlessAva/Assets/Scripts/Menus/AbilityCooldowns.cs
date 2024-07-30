using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldowns : MonoBehaviour
{
    public Attack attackScript;
    private Image image;
    [SerializeField] private Text text;
    [SerializeField] private GameObject abilityText;
    private float currentCooldown;

    void Start()
    {
        image = GetComponent<Image>();

        if (name == "IceAbility")
        {
            currentCooldown = attackScript.iceAttackCooldown;
        }
        else if (name == "FireAbility")
        {
            currentCooldown = attackScript.fireAttackCooldown;
        }
        else if (name == "ShieldAbility")
        {
            currentCooldown = attackScript.shieldCooldown;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (name == "IceAbility")
        {
            if (!attackScript.CanAttack(attackScript.lastIceAttackTime, attackScript.iceAttackCooldown))
            {
                if(currentCooldown > 0)
                {
                    currentCooldown -= Time.deltaTime;
                }
                else
                {
                    currentCooldown = 0;
                }

                abilityText.SetActive(true);
                text.text = Mathf.Abs(currentCooldown).ToString();
                image.color = Color.grey;
            }
            else
            {
                abilityText.SetActive(false);
                text.text = "";
                currentCooldown = attackScript.iceAttackCooldown;
                image.color = Color.white;
            }
        }
        else if (name == "FireAbility")
        {
            if (!attackScript.CanAttack(attackScript.lastFireAttackTime, attackScript.fireAttackCooldown))
            {
                if (currentCooldown > 0)
                {
                    currentCooldown -= Time.deltaTime;
                }
                else
                {
                    currentCooldown = 0;
                }

                abilityText.SetActive(true);
                text.text = Mathf.Abs(currentCooldown).ToString();
                image.color = Color.grey;
            }
            else
            {
                abilityText.SetActive(false);
                text.text = "";
                currentCooldown = attackScript.fireAttackCooldown;
                image.color = Color.white;
            }
        }
        else if (name == "ShieldAbility")
        {
            if (!attackScript.CanUseShield(attackScript.lastShieldUseTime, attackScript.shieldCooldown))
            {
                if (currentCooldown > 0)
                {
                    currentCooldown -= Time.deltaTime;
                }
                else
                {
                    currentCooldown = 0;
                }

                abilityText.SetActive(true);
                text.text = Mathf.Abs(currentCooldown).ToString();
                image.color = Color.grey;
            }
            else
            {
                abilityText.SetActive(false);
                text.text = "";
                currentCooldown = attackScript.shieldCooldown;
                image.color = Color.white;
            }
        }
    }
}
