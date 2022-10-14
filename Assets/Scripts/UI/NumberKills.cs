using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberKills : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _numberKills;

    private void Start()
    {
        _numberKills.text = "0";
    }

    private void OnEnable()
    {
        _player.ChangedNumberKills += OnNumberKillsChanged;
    }

    private void OnDisable()
    {
        _player.ChangedNumberKills += OnNumberKillsChanged;
    }

    private void OnNumberKillsChanged(int numberKills)
    {
        _numberKills.text = numberKills.ToString();
    }
}
