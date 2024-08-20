using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public Destroyable destroyable;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollisionWithRock()
    {
        FindObjectOfType<AudioManagement>().PlaySound("DragonRoar");
        Debug.Log("The Dragon has been defeated! *Airhorn Noises*");
        destroyable.Destroy();
    }


}
