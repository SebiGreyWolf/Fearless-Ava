using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PatrolingEnemy : MonoBehaviour
{
    public Player player;
    public GameObject playerPrefab;

    public float detectionRange = 5;
    private float detectionAngle = 45.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerInRange();
    }

    bool isPlayerInRange() 
    { 
        if(this != null && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= detectionRange)
            {
                Vector3 directionToTarget = (player.transform.position - transform.position).normalized;
                Vector3 referenceForward = transform.right * transform.localScale.x;
                float angle = Vector3.Angle(referenceForward, directionToTarget);


                if (angle < detectionAngle)
                {
                    Debug.Log("Detected");
                    Debug.DrawRay(transform.position, directionToTarget * distance);
                    return true;
                }
            }
        }
        return false; 
    }


    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.position;
        Vector3 forward = transform.right;

        Debug.DrawRay(position, forward, Color.yellow);

        Vector3 test = new Vector3(1, 1, 0);
        Vector3 test2 = new Vector3(1, -1, 0);

        // Zeichne die Ränder des Sichtkegels
        Quaternion leftRayRotation = Quaternion.AngleAxis(-detectionAngle, test2 * transform.localScale.x);
        Quaternion rightRayRotation = Quaternion.AngleAxis(detectionAngle, test * transform.localScale.x);

        Vector3 leftRayDirection = leftRayRotation * forward;
        Vector3 rightRayDirection = rightRayRotation * forward;

        Gizmos.DrawRay(position, leftRayDirection * detectionRange);
        Gizmos.DrawRay(position, rightRayDirection * detectionRange);
    }
    */
}
