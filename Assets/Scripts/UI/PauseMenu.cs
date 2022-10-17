using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void OpenMenu()
    {
        _pauseMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        _pauseMenu.SetActive(false);
    }

    public void QuitFromGame()
    {
        Application.Quit();
    }
}
