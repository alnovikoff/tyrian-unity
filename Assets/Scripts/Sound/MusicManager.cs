using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public Music[] musics;
    public AudioSource aus;
    void Awake()
    {
        foreach (Music s in musics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
        aus = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "level1")
        {
            FindObjectOfType<MusicManager>().PlayMusic("track1");
        }
        else if (SceneManager.GetActiveScene().name == "level2")
        {
            FindObjectOfType<MusicManager>().PlayMusic("track2");
        }
        else if (SceneManager.GetActiveScene().name == "level3")
        {
            FindObjectOfType<MusicManager>().PlayMusic("track3");
        }
    }

    public void PlayMusic(string name)
    {
        Music s = Array.Find(musics, musics => musics.name == name);
        s.source.volume = 0.05f;
        s.source.Play();

        StartCoroutine(SmoothStart(aus));

        s.source.loop = true;
    }

    public void StopAllMusic()
    {
        aus.Stop();
    }

    private IEnumerator SmoothStart(AudioSource s)
    {
        while (s.volume < 0.25f)
        {
            yield return new WaitForSeconds(0.5f);
            s.volume += 0.01f;
        }
    }
}