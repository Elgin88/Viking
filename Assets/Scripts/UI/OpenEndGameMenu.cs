using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEndGameMenu : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _endGameMenu;

    private void Update()
    {
        if (_player == null)
        {
            _endGameMenu.SetActive(true);
        }        
    }
}
