using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Animation anim;
    [SerializeField] FadeEffect fadeEffect;


    private void Start()
    {
        Time.timeScale = 1;
        anim.Stop("fky_away");
        anim.Play("idle_menu_anim");
    }
    public void OnPlayButton()
    {
        StartCoroutine(fadeEffect.FadeIn());
        anim.Stop("idle_menu_anim");
        anim.Play("fky_away");
        Invoke("Rungame", 3);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    private void Rungame()
    {
        SceneManager.LoadScene("level1");
    }
}
