using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    public SoundClass[] soundClasses;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (SoundClass s in soundClasses)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;

            //Wärend dem Testen (Level1)
            //s.audioSource.volume = s.volume;
            //Bei Builds (Start vom Main Menu)
            s.audioSource.volume = AudioListener.volume;

            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
    }

    public void PlaySound(string name)
    {
        //Debug.Log("Playing Sound!");

        SoundClass s = Array.Find(soundClasses, sound => sound.name == name);
        //Debug.Log("Playing Sound:" + s.name);
        if (s == null)
            return;

        s.audioSource.Play();
    }

    public void PauseSound(string name)
    {
        SoundClass s = Array.Find(soundClasses, sound => sound.name == name);
        //Debug.Log("Playing Sound:" + s.name);
        if (s == null)
            return;

        s.audioSource.Pause();
    }

    public void UnPauseSound(string name)
    {
        SoundClass s = Array.Find(soundClasses, sound => sound.name == name);
        //Debug.Log("Playing Sound:" + s.name);
        if (s == null)
            return;

        s.audioSource.UnPause();
    }
}
