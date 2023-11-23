using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreCrash, scoreHits, scoreKills, credCrash, credKills, credHits;
    [SerializeField] public PlayerBase playerBase;
    [SerializeField] GameObject winUI;

    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        if (winUI != null)
            winUI.SetActive(false);
        RefreshCurrncies();
    }

    public void RefreshCurrncies()
    {
        scoreCrash.text = Currencies.Instance.scoreCrashInt.ToString();
        scoreHits.text = Currencies.Instance.scoreHitsInt.ToString();
        scoreKills.text = Currencies.Instance.scoreKillsInt.ToString();
        credCrash.text = Currencies.Instance.credCrashInt.ToString();
        credHits.text = Currencies.Instance.credHitsInt.ToString();
        credKills.text = Currencies.Instance.credKillsInt.ToString();
    }

    public void WinUI()
    {
        winUI.SetActive(true);
    }
}
