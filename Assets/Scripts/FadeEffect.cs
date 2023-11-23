using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public Image blackScreen;
    public float fadeDuration = 3.0f;

    private void Start()
    {
        blackScreen.gameObject.SetActive(false);
    }

    public IEnumerator FadeIn()
    {
        blackScreen.gameObject.SetActive(true);

        while (blackScreen.color.a != 255)
        {
            var newAlpha = Mathf.MoveTowards(blackScreen.color.a, 255, 0.5f * Time.deltaTime);
            blackScreen.color = new Color(0,0, 0, newAlpha);
            yield return null;
        }
        blackScreen.gameObject.SetActive(false);
    }
}

