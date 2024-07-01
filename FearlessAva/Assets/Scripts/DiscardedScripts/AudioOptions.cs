using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    public Slider volumeSlider; // Referenz auf den Slider
    public AudioMixer audioMixer; // Referenz auf den AudioMixer
    private float volume;

    void Start()
    {

        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 100f;

        audioMixer.GetFloat("Master", out volume);
        volumeSlider.value = volume;

        Debug.Log("Volume Gotten: " + volume);


        //Listener, der auf Veränderungen des Sliders reagiert
        volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
    }

    void ChangeVolume()
    {
        float sliderValue = volumeSlider.value;

        volume = Mathf.Lerp(-80f, 0f, sliderValue / 100);

        Debug.Log("Volume on Change: " + volume);

        audioMixer.SetFloat("Master", volume);


        float test;
        audioMixer.GetFloat("Master", out test);
        Debug.Log("Volume on Change Test: " + test);
    }
}
