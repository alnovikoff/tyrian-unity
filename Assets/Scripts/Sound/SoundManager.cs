using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject soundOn;
    [SerializeField] GameObject soundOff;
    [SerializeField] GameObject musicOn;
    [SerializeField] GameObject musicOff;

    public bool isSFXMuted = false;
    public bool isBGMMuted = false;

    [SerializeField] AudioMixer masterMixer;

    [SerializeField] string sfxVolume = "musicVol";
    [SerializeField] string miscVolume = "SFXVol";

    float maxLevel = 0f;
    float minLevel = -80f;




    public void MuteSFXVolume()
    {
        FindObjectOfType<AudioManager>().PlaySound("buttonClick");
        isSFXMuted = !isSFXMuted;
        if (isSFXMuted)
        {
            masterMixer.SetFloat(sfxVolume, minLevel);
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
        else
        {
            masterMixer.SetFloat(sfxVolume, maxLevel);
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
    }

    public void MuteMusicVolume()
    {
        FindObjectOfType<AudioManager>().PlaySound("buttonClick");
        isBGMMuted = !isBGMMuted;
        if (isBGMMuted)
        {
            masterMixer.SetFloat(miscVolume, minLevel);
            musicOn.SetActive(false);
            musicOff.SetActive(true);
        }
        else
        {
            masterMixer.SetFloat(miscVolume, maxLevel);
            musicOn.SetActive(true);
            musicOff.SetActive(false);
        }
    }
}
