using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    public static Cheats Instance;
    public bool isCheatsAcivated;
    public TMP_Text cheatTxt;
    private void Awake()
    {
        // Check, if we do not have any instance yet.
        if (Instance == null)
        {
            // 'this' is the first instance created => save it.
            Instance = this;
        }
        else if (Instance != this)
        {
            // Destroy 'this' object as there exist another instance
            Destroy(this.gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        isCheatsAcivated = false;
        CheckCheatUI();
    }

    void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetKeyDown("c"))
        {
            
            if (!isCheatsAcivated)
            {
                isCheatsAcivated = true;
                Debug.Log("Cheats were ativated");
            }
            else
            {
                Debug.Log("Cheats were deactivated");
                isCheatsAcivated = false;
            }
            CheckCheatUI();
        }
#endif
    }

    void CheckCheatUI()
    {
        if(!isCheatsAcivated)
        {
            cheatTxt.color = Color.green;
            cheatTxt.text = "deactivated";
        }
        else
        {
            cheatTxt.color = Color.red;
            cheatTxt.text = "activated";
        }
    }
}
