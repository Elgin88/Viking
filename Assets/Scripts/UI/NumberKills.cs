using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class NumberKills : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _numberOfKills;

    private void Start()
    {
        _numberOfKills.text = "0";
    }

    private void OnEnable()
    {
        _player.ChangedNumberKills += OnEnemyKills;        
    }

    private void OnDisable()
    {
        _player.ChangedNumberKills -= OnEnemyKills;
    }

    private void OnEnemyKills(int numberKills)
    {
        _numberOfKills.text = numberKills.ToString();
    }
}
