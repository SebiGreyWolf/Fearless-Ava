using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dragon : MonoBehaviour
{
    public Destroyable destroyable;

    public void CollisionWithRock()
    {
        FindObjectOfType<AudioManagement>().PlaySound("DragonRoar");
        Debug.Log("The Dragon has been defeated! *Airhorn Noises*");

        StartCoroutine(SkipToNextScene());

        destroyable.Destroy();
    }

    IEnumerator SkipToNextScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("EndScene");
    }

}
