using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Currencies : MonoBehaviour
{
    public static Currencies Instance;

    [SerializeField] public int scoreCrashInt, scoreHitsInt, scoreKillsInt, credCrashInt, credKillsInt, credHitsInt;

    [SerializeField] public int coef = 2;

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
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
