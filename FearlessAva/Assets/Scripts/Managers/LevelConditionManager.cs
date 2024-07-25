using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConditionManager : MonoBehaviour
{
    public GameObject objectToDestroy;
    public GameObject object1;
    public GameObject object2;

    private bool object1Destroyed = false;
    private bool object2Destroyed = false;

    void Update()
    {
        // Check if object1 is destroyed
        if (object1 == null && !object1Destroyed)
        {
            object1Destroyed = true;
            CheckAndDestroyObject();
        }

        // Check if object2 is destroyed
        if (object2 == null && !object2Destroyed)
        {
            object2Destroyed = true;
            CheckAndDestroyObject();
        }
    }

    private void CheckAndDestroyObject()
    {
        if (object1Destroyed && object2Destroyed)
        {
            Destroy(objectToDestroy);
        }
    }
}
