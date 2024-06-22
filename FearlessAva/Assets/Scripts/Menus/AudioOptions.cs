using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    public Slider volumeSlider; // Referenz auf den Slider
    public AudioMixer audioMixer; // Referenz auf den AudioMixer

    void Start()
    {
        float volume;

        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 100f;

        audioMixer.GetFloat("MasterVolume", out volume);
        volumeSlider.value = volume;

        //Listener, der auf Veränderungen des Sliders reagiert
        volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
    }

    void ChangeVolume()
    {
        float sliderValue = volumeSlider.value;

        float volume = Mathf.Lerp(-80f, 0f, sliderValue / 100);

        audioMixer.SetFloat("MasterVolume", volume);
    }
}
