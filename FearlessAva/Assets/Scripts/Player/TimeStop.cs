using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool restoreTime;
    [SerializeField]
    private float timescale = 1f;

    void Start()
    {
        restoreTime = false;
    }

    void Update()
    {
        // No need to handle time scale changes here
    }

    public void StopTime(float changeTime, float restoreSpeed, float delay)
    {
        speed = restoreSpeed;
        Time.timeScale = changeTime;

        if (delay > 0)
        {
            StopCoroutine("StartTimeAgain");
            StartCoroutine(StartTimeAgain(delay));
        }
        else
        {
            StopCoroutine("RestoreTime");
            StartCoroutine(RestoreTime());
        }
    }

    IEnumerator StartTimeAgain(float amount)
    {
        yield return new WaitForSecondsRealtime(amount);
        StartCoroutine(RestoreTime());
    }

    IEnumerator RestoreTime()
    {
        while (Time.timeScale < timescale)
        {
            Time.timeScale += speed * Time.unscaledDeltaTime;
            if (Time.timeScale > timescale)
            {
                Time.timeScale = timescale;
            }
            yield return null;
        }
        restoreTime = false;
    }
}
