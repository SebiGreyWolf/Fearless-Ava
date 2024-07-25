using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerManager : MonoBehaviour
{
    public Animator playerAnimator;
    public Animator otherObjectAnimator;
    public GameObject player;
    public GameObject otherObject; // Reference to the other object to be destroyed
    public string playerAnimationName;
    public string otherObjectAnimationName;
    public MonoBehaviour playerMovementScript; // Reference to the player's movement script

    private bool animationPlaying = false;
    private Rigidbody2D playerRb;

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!animationPlaying && other.gameObject == player)
        {
            animationPlaying = true;

            // Stop player movement
            playerMovementScript.enabled = false;
            playerRb.velocity = Vector2.zero;
            playerRb.isKinematic = true;

            // Play player animation
            playerAnimator.Play(playerAnimationName);

            // Play other object's animation
            otherObjectAnimator.Play(otherObjectAnimationName);

            // Start coroutine to re-enable movement and destroy the other object after animations are done
            StartCoroutine(ReEnableMovementAndDestroyOtherObject());
        }
    }

    private IEnumerator ReEnableMovementAndDestroyOtherObject()
    {
        // Wait for the length of the player's animation
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Re-enable player movement
        playerRb.isKinematic = false;
        playerMovementScript.enabled = true;
        animationPlaying = false;

        // Wait for the length of the other object's animation
        yield return new WaitForSeconds(otherObjectAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Destroy the other object
        Destroy(otherObject);
        Destroy(this);
    }
}
