using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = 0.7f;
        s.source.Play();
    }
}
