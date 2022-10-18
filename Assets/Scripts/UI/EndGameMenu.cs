using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject _endGameMenu;
    [SerializeField] private TMP_Text _numberKills;
    [SerializeField] private Player _player;

    private string _nameOfGameScene = "SampleScene";

    private void OnEnable()
    {
        Time.timeScale = 1;
        _numberKills.text = _player.CurrentNumberKills.ToString();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void ExitFromGame()
    {
        Application.Quit();
    }

    public void RepeatGame()
    {
        SceneManager.LoadScene(_nameOfGameScene);
    }
}
