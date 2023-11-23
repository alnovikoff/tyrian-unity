using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] GameObject img;
    [SerializeField] AudioSource audioSource1;
    [SerializeField] AudioClip[] audios;

    public void OnEnterAnimationEvent()
    {
        audioSource1.clip = audios[0];
        audioSource1.Play();
    }

    public void OnAEnterAnimationEvent()
    {
        audioSource1.clip = audios[1];
        audioSource1.Play();
        img.SetActive(true);
    }

    public void OnExitAnimationEvent()
    {
        img.SetActive(false);
        GetComponent<Animator>().enabled = false;
    }
}
