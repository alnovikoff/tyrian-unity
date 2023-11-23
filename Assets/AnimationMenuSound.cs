using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMenuSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource1;
    [SerializeField] AudioClip audioClip;

    public void OnEnterAnimationEvent()
    {
        audioSource1.clip = audioClip;
        audioSource1.Play();
    }
}
