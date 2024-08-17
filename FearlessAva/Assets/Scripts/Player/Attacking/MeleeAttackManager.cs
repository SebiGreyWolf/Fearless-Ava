using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackManager : MonoBehaviour
{
    public float defaultForce = 40;
    public float upwardsForce = 40;
    public float movementTime = .1f;
    private bool meleeAttack;
    private Animator meleeAnimator;
    private PlayerMovement character;

    private void Start()
    {
        character = GetComponent<PlayerMovement>();
        meleeAnimator = GetComponentInChildren<MeleeWeapon>().gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            meleeAttack = true;
            if (GetComponentInChildren<MeleeWeapon>())
                FindObjectOfType<AudioManagement>().PlaySound("Attack");
        }
        else
        {
            meleeAttack = false;
        }

        if (meleeAttack && Input.GetAxis("Vertical") > 0)
        {
            meleeAnimator.SetTrigger("UpwardMeleeSwipe");
        }
        else if (meleeAttack && Input.GetAxis("Vertical") < 0 && !character.IsGrounded)
        {
            meleeAnimator.SetTrigger("DownwardMeleeSwipe");
        }
        else if ((meleeAttack && Input.GetAxis("Vertical") == 0) ||
                  (meleeAttack && Input.GetAxis("Vertical") < 0 && character.IsGrounded))
        {
            meleeAnimator.SetTrigger("ForwardMeleeSwipe");
        }
    }
}