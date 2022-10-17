using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberBullets : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _numberBullets;

    private void Start()
    {
        _numberBullets.text = _player.MaxNumberBullets.ToString();
    }

    private void OnEnable()
    {
        _player.ChangedNumberBullets += OnNumberBulletsChange;        
    }

    private void OnDisable()
    {
        _player.ChangedNumberBullets -= OnNumberBulletsChange;
    }

    private void OnNumberBulletsChange(int numberBullets)
    {
        _numberBullets.text = numberBullets.ToString();
    }
}