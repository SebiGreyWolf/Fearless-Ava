using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void ParallaxCameraDelegate(float deltaMovement);
public class ParallaxCamera : MonoBehaviour
{
    public ParallaxCameraDelegate onCameraTranslate;

    private float oldPosition;

    void Start()
    {
        oldPosition = transform.position.x;
    }

    void Update()
    {
        if (transform.position.x != oldPosition)
        {
            if (onCameraTranslate != null)
            {
                float delta = oldPosition - transform.position.x;
                onCameraTranslate(delta);
            }

            oldPosition = transform.position.x;
        }
    }
}
