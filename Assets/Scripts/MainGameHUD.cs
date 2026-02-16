using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MainGameHUD : MonoBehaviour {
    [SerializeField, Tooltip("TMP object displaying our current health.")]
    TextMeshProUGUI _healthValueText;

    [SerializeField, Tooltip("TMP object displaying the number of collected coins.")]
    TextMeshProUGUI _coinValueText;

    [SerializeField, Tooltip("TMP object displaying lives remaining.")]
    TextMeshProUGUI _livesValueText;

    [SerializeField, Tooltip("The Health Manager we're displaying data for.")]
    HealthManager _healthManager;

    void Update() {
        int curHealth = Mathf.RoundToInt(_healthManager.GetHealthCur());
        int maxHealth = Mathf.RoundToInt(_healthManager.GetHealthMax());
        _healthValueText.text = curHealth + "/" + maxHealth;

        _coinValueText.text = GameSessionManager.Instance.GetCoins().ToString();

        _livesValueText.text = GameSessionManager.Instance.GetLives().ToString();
    }
}
