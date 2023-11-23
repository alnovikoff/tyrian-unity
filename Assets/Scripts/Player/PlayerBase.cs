using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] public int maxHealth = 30;
    [SerializeField] public int currentHealth = 0;
    [SerializeField] public int playerBulletDamage = 2;
    [SerializeField] public int playerDamage = 30;

    [SerializeField] public TMP_Text healthTxt;
    [SerializeField] public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthTxt.text = currentHealth.ToString();
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
    } 

    public void RefreshUI()
    {
        healthTxt.text = currentHealth.ToString();
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
    }
}
