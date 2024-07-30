using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReworkedMeleeAttackManager : MonoBehaviour
{
    public float defaultForce = 300;
    public float upwardsForce = 600;
    public float movementTime = .1f;
    private bool meleeAttack;
    private Animator meleeAnimator;
    private ReworkedPlayerMovement character;

    private void Start()
    {
        character = GetComponent<ReworkedPlayerMovement>();
        meleeAnimator = GetComponentInChildren<ReworkedMeeleWeapon>().gameObject.GetComponent<Animator>();
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
