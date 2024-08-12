using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackManager : MonoBehaviour
{public float defaultForce = 300;
    public float upwardsForce = 600;
    public float movementTime = 0.1f;
    private bool meleeAttack;
    public Animator meleeAnimator;

    private Animator anim;
    private PlayerMovement character;

    private void Start()
    {
        anim = GetComponent<Animator>();
        character = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        meleeAttack = Input.GetKeyDown(KeyCode.Mouse0);

        if (meleeAttack)
        {
            float verticalInput = Input.GetAxisRaw("Vertical"); // Use GetAxisRaw for more immediate response

            if (verticalInput > 0)
            {
                // Immediate upward attack
                meleeAnimator.SetTrigger("UpwardMeleeSwipe");
            }
            else if (verticalInput < 0 && !character.IsGrounded)
            {
                // Immediate downward attack
                meleeAnimator.SetTrigger("DownwardMeleeSwipe");
            }
            else
            {
                // Immediate forward or downward (grounded) attack
                meleeAnimator.SetTrigger("ForwardMeleeSwipe");
            }
        }
    }
}
