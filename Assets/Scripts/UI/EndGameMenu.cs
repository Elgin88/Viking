using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject _endGameMenu;

    private void OnEnable()
    {
        Time.timeScale = 0;        
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    private void ExitFromGame()
    {
        Application.Quit();
    }

    private void RepeatGame()
    {

    }
}
